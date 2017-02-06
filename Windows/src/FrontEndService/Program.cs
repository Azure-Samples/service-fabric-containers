// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace FrontEndService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    #region serializationTypes

    [DataContract]
    class AddressResult
    {
        [DataMember(Name = "Endpoints")] public Dictionary<string, string> Endpoints;
    }

    [DataContract]
    class EndpointsResult
    {
        [DataMember(Name = "Kind")] public int Kind;

        [DataMember(Name = "Address")] public string Address;
    }

    [DataContract]
    class ResolveEndpointResult
    {
        [DataMember(Name = "Name")] public string Name;

        [DataMember(Name = "Endpoints")] public List<EndpointsResult> Endpoints;

        [DataMember(Name = "Version")] public string Version;
    }

    #endregion

    public class FrontEndService
    {
        private string gatewayPort;
        private string gatewayAddress;
        private string previousRsp;
        HttpListener listener;
        string serviceName;
        bool isContainer;

        public FrontEndService(string gatewayPort, string serviceName, bool isContainer)
        {
            this.gatewayPort = gatewayPort;
            this.previousRsp = null;
            this.isContainer = isContainer;
            // Create the REST URL to talk to the Naming Service. The URL is different depending on whether the code is in a container
            // due to a bus in the Window Server 2016 container networking 
            if (!isContainer)
            {
                //Get the environment variable for IP address/FQDN of the node
                string nodeIPAddress = Environment.GetEnvironmentVariable("Fabric_NodeIPOrFQDN");
                this.gatewayAddress = String.Format("http://{0}:{1}/Services/{2}/$/ResolvePartition?api-version=1.0", nodeIPAddress, gatewayPort, serviceName);
            }
            else
            {
                this.gatewayAddress = String.Format(
                    "http://{0}:{1}/Services/{2}/$/ResolvePartition?api-version=1.0",
                    this.GetInternalGatewayAddress(),
                    gatewayPort,
                    serviceName);
            }
            this.serviceName = serviceName;
        }

        //Start an HttpListener for the FrontEndService listening on port 80.
        public void StartupListener()
        {
            string listenAddress;
            try
            {
                this.listener = new HttpListener();
                // Port 80 fixed and exposed in the FrontEndService dockerfile
                // Need to have different listen addresses depending on whether in a container of not. 
                // This is temporary due to Windows Server 2016 networking fix coming in a later build.
                if (this.isContainer)
                {
                    listenAddress = String.Format("http://{0}:80/", this.GetIpAddress());
                }
                else
                {
                    listenAddress = "http://+:80/";
                }
                this.listener.Prefixes.Add("http://+:80/");
                Console.WriteLine("Listen address is {0}", listenAddress);
                this.listener.Start();
                Console.WriteLine("HttpListener started");
                string nodeName = Environment.GetEnvironmentVariable("Fabric_NodeName");
                Console.WriteLine("Node Name is {0}", nodeName);
                string message = String.Format("\nFront End Node Name: {0}\n", nodeName);
                byte[] messageBuf = System.Text.Encoding.UTF8.GetBytes(message);

                while (true)
                {
                    try
                    {
                        HttpListenerContext context = this.listener.GetContext();
                        HttpListenerRequest request = context.Request;
                        List<string> endpoints = this.GetEndpointAddresses();
                        Console.WriteLine("1st Endpoint Address {0}", endpoints[0]);
                        int index = 0;
                        while (index < endpoints.Count)
                        {
                            try
                            {
                                // Call the BackEndService
                                string response = this.GetResponseFromBackend(endpoints[index]);
                                Console.WriteLine("Writing response from backend service to client browser {0}", response);
                                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(response);
                                context.Response.ContentLength64 = buffer.Length;
                                System.IO.Stream output = context.Response.OutputStream;
                                output.Write(buffer, 0, buffer.Length);

                                //Write out the Front End Node Name into the response
                                output.Write(messageBuf, 0, messageBuf.Length);
                                output.Close();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(
                                    "Unable to connect to endpoint address {0} index {1} total addresses {2}, Exception {3}",
                                    endpoints[index],
                                    index,
                                    endpoints.Count,
                                    e.Message);
                                index++;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception in GetContext {0}", e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured in startup listener {0}", e.Message);
            }
        }

        // Temporary workaround for Windows Server 2016 networking. See https://blogs.technet.microsoft.com/virtualization/2016/05/25/windows-nat-winnat-capabilities-and-limitations/
        // Due to a bug you cannot call the host public IP Address from within a container. The workaround is to find the address
        // of the gateway in the container and use this to communicate to the host. This will be fixed in a future Windows Server 2016 release
        private string GetInternalGatewayAddress()
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.GetIPProperties().GatewayAddresses != null &&
                    networkInterface.GetIPProperties().GatewayAddresses.Count > 0)
                {
                    foreach (GatewayIPAddressInformation gatewayAddr in networkInterface.GetIPProperties().GatewayAddresses)
                    {
                        if (gatewayAddr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return gatewayAddr.Address.ToString();
                        }
                    }
                }
            }
            throw new ArgumentNullException("internalgatewayaddress");
        }

        //Get the listening IP address of the host for the Frontend service
        private string GetIpAddress()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = hostEntry.AddressList.FirstOrDefault(
                ip =>
                    (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            if (ipAddress != null)
            {
                return ipAddress.ToString();
            }
            throw new InvalidOperationException("HostIpAddress");
        }

        //Calls the Service Fabric Naming Service using the this.gatewayAddress set in the FrontEndService() contructor
        //to get a list of Endpoint addresses for the BackEndService instance registered with the Naming Service.
        //This demonstrates calling the ResolvePartition over REST for the the Naming Service.
        private List<string> GetEndpointAddresses()
        {
            List<string> addresses = new List<string>();
            ResolveEndpointResult result = new ResolveEndpointResult();
            try
            {
                StringBuilder endpointAddress = new StringBuilder(this.gatewayAddress);
                if (!String.IsNullOrEmpty(this.previousRsp))
                {
                    endpointAddress.AppendFormat("&PreviousRspVersion={0}", this.previousRsp);
                }
                Console.WriteLine("Gateway address {0}", endpointAddress.ToString());
                HttpWebRequest resolveRequest = (HttpWebRequest) WebRequest.Create(endpointAddress.ToString());
                resolveRequest.KeepAlive = false;
                resolveRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse) resolveRequest.GetResponse();
                Console.WriteLine("GetResponse completed");
                if (response.StatusCode == HttpStatusCode.Accepted ||
                    response.StatusCode == HttpStatusCode.OK)
                {
                    Stream recvStream = response.GetResponseStream();
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ResolveEndpointResult));
                    Console.WriteLine("Deserializing");
                    result = (ResolveEndpointResult) ser.ReadObject(recvStream);
                    this.previousRsp = result.Version;
                    Console.WriteLine("Deserialized address {0}", result.Endpoints[0].Address);
                    AddressResult serviceEndpoints = null;
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(
                        typeof(AddressResult),
                        new DataContractJsonSerializerSettings() {UseSimpleDictionaryFormat = true});

                    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(result.Endpoints[0].Address));
                    serviceEndpoints = (AddressResult) deserializer.ReadObject(stream);
                    addresses.AddRange(serviceEndpoints.Endpoints.Values);
                }
                else
                {
                    Console.WriteLine("Recieved status code {0}", response.StatusCode);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured {0}", e.Message);
            }
            return addresses;
        }

        // Call the BackEndService.js Nodejs application with the address returned for each endpoint. 
        // Demonstrates calling a Nodejs application from a C# application
        private string GetResponseFromBackend(string endpointAddress)
        {
            try
            {
                Console.WriteLine("Getting response from {0}", endpointAddress);
                string resp = null;
                HttpWebRequest resolveRequest = (HttpWebRequest) WebRequest.Create(endpointAddress);
                resolveRequest.KeepAlive = false;
                resolveRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse) resolveRequest.GetResponse();
                Console.WriteLine("GetResponse completed");
                if (response.StatusCode == HttpStatusCode.Accepted ||
                    response.StatusCode == HttpStatusCode.OK)
                {
                    Stream recvStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(recvStream);
                    resp = reader.ReadToEnd();
                }
                return resp;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured in GetResponseFrombackend Request {0}", e.Message);
                throw;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Uncomment the line below to enable debugging of the FrontEndService
            //System.Diagnostics.Debugger.Launch();
            //Get environment variables passed into the service from the Service.xml manifest to enable the FrontEndService to communicate to the BackEndService
            //and indicate whether the code is running in Windows Container 
            string serviceName = Environment.GetEnvironmentVariable("BackendServiceName");
            string httpGatewayPort = Environment.GetEnvironmentVariable("HttpGatewayPort");
            bool isContainer = Convert.ToBoolean(Environment.GetEnvironmentVariable("IsContainer"));
            Console.WriteLine("ServiceName {0}, GatewayPort {1}", serviceName, httpGatewayPort);
            FrontEndService frontEnd = new FrontEndService(httpGatewayPort, serviceName, isContainer);
            frontEnd.StartupListener();
            Console.ReadKey();
        }
    }
}