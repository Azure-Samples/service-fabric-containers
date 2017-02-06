---
services: service-fabric
platforms: dotnet
author: mfussell
---

# Service Fabric Container Samples
This repository contains sample projects to help you get started with Service Fabric and Containers both on Windows and Linux.

## Windows Server 2016 Container Sample
This sample shows containers running on Windows Server 2016. The solution has a frontend service written in C# that listens for web requests on port 80 and
a backend service written in nodejs that serves up a web page with information about the node it is running on.

The solution contains two projects. One deploys the services as guest executables. The other uses dockerfiles to build containerized versions of the services.
In this way you can compare and contrast the difference between deploying a services as an EXEs or as containers.

## Prerequisites for development machine
1. Get a physical machine or Azure VM with "Windows Server 2016 with containers" for your development machine. 
2. Install [Nodejs tools for Visual Studio](https://www.visualstudio.com/vs/node-js/)  
3. Install [Nodejs 6.9.1 x64 runtime](https://nodejs.org/en/) (Note: You can choose other nodejs runtime versions, but you will need to change the dockerfile accordingly in the BackendService project)
4. Install the Service Fabric SDK (version 5.4.x and above)
5. Clone or download this container solution into a directory from here onwards called **[mydirectory]**

## How to Build and Deploy the GuestExe.Application
1. Build the VS solution
2. Select the GuestExe.Application project, right click and select **Publish** to publish the GuestExe.Application via Visual Studio. Wait for services to deploy and start.
3. Open a browser and browse to **http://localhost**. You should see a green web page displayed with the name of the node the BackendService is running on.

## How to Build and Deploy the Container.Application
1. Build the VS solution

2. Open a console prompt at C:\[mydirectory]\Container.Application\Container.Application\FrontEndService and run the following docker
command replacing [myrepo] with the name of your dockerhub repo and add a version tag to the image e.g. v1, v2

```xml
docker build --tag=”[myrepo]/servicefabricfrontendservice:v1” --file=”FrontEndService.dockerfile” .
```

NOTE: In the 5.4 release of Service Fabric runtime, when a container is deployed to Service Fabric, to communicate with docker you need to use a local host port by adding -H localhost:2375 to all your docker commmands.
If you see *"error during connect: Get http://%2F%2F.%2Fpipe%2Fdocker_engine/v1.25/containers/json: open //./pipe/docker_engine: The system cannot find the file specified."* then instead do the following command;

```xml
docker -H localhost:2375 build --tag=”[myrepo]/servicefabricfrontendservice:v1” --file=”FrontEndService.dockerfile” .
```

3. Download NodeJs node-v6.9.1-x64.msi [from nodejs](https://nodejs.org/en/) (or pick a version of your choice) and copy this into the 
C:\[mydirectory]\Container.Application\Container.Application\NodejsBackEndService\sources directory.

4. Go up one directory level to C:\[mydirectory]\Container.Application\Container.Application\NodejsBackEndService and run the following docker command.

```xml
docker build --tag=”[myrepo]/servicefabricbackendservice:v1” --file=”BackEndService.dockerfile” .
```

5. Check that the images for both containers have been created in the local repository by running the docker command.

```xml
docker images
```

6. Next login to your docker hub repo providing your credentials and push the images with the following docker commands. 

```xml
docker  login
docker push [myrepo]/servicefabricfrontendservice:v1
docker push [myrepo]/servicefabricbackendservice:v1
```

7. Select the Container.Application project, right click and select **Publish** to publish the Container.Application via Visual Studio. Wait for services to deploy and start.

**Note:** If you want to validate whether the services are running on your local machine, unfortunatelyyou can't just open up **http://localhost** in your browser at this time due to
[an issue in WinNAT](https://blogs.technet.microsoft.com/virtualization/2016/05/25/windows-nat-winnat-capabilities-and-limitations/). However you can use the IP Address of the Windows Container. 
To find that, type:

```xml
docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" c8e5 
```

Note that 'c8e5' is the start of a running Containers' Container ID, which will be different in your situation.
It shows the IP Address of your running container for the FrontendService, for example 172.20.116.222. In your environment it will be different.
Open up Internet Explorer and navigate to that IP Address, and you should see a green web page.

8. Alternatively create a cluster on Azure with a "Windows 2016 with containers" image and deploy the 
project to this cluster. Since all the calls here go via the Azure load balancer, you can call onto the FrontendService.
Open a browser and browse to **http://[my.azurecluster.name]:80** and you should see a green web page displayed with the name of the node the BackendService is running on.

## More information
The [Service Fabric documentation][service-fabric-docs] includes a rich set of tutorials and conceptual articles, which serve as a good complement to the samples.
The [Service Fabric container documentation][service-fabric-containers-overview] provides details on the container features and scenarios.

 Loek has these excellent blog posts on Service Fabric and Windows Containers 
- [Running Windows Containers on Azure Service Fabric](https://loekd.blogspot.com/2017/01/running-windows-containers-on-azure.html)
- [Running Windows Containers on Azure Service Fabric Part II](https://loekd.blogspot.com/2017/01/running-windows-containers-on-azure_10.html)


## Debugging notes
1. If you see "Access denied" from a service this usually means that you have not ACLed the endpoint with http.sys. 
Ensure that protocol="http" is in the Endpoint defintion in the ServiceManiest.xml 

2. To debug docker logs get the CONTAINER ID with

```xml
docker ps
```
 
Then get logs for the container instance 

```xml
docker logs <CONTAINER ID> 
```

3. To run the container locally and see whether it runs successfully use

```xml
docker -H localhost:2375  run -ti [myrepo]/servicefabricbackendservice:v1
```

<!-- Links -->

[service-fabric-docs]: http://aka.ms/servicefabricdocs
[service-fabric-containers-overview]: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-containers-overview/
[service-fabric-samples]: http://aka.ms/servicefabricsamples
