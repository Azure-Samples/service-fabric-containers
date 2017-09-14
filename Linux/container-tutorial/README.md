---
services: service-fabric
platforms: dotnet
author: msfussell
---

# Service Fabric Container Samples
This repository contains a sample project to help you get started with Service Fabric and Containers on Linux.

# Linux Ubuntu 16.04 Container Sample
The solution contains two folders and a 'docker-compse.yml' file. The 'azure-vote' folder contains the Python frontend service along with the Dockerfile used to build the image. The 'redis' directory contains the Dockerfile used to build the backend image. The 'docker-compose.yml' file is used to deploy the application to Service Fabric. 

## Prerequisites for development machine
1. Get a physical machine or Azure VM with "Ubuntu 16.04" for your development machine. 
2. [Set up the developer environment](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started-linux) 
3. Create a Service Fabric Linux cluster on Azure with a minimum of five nodes. 
    NOTE: The Linux demo requires a cluster running on Azure. For the purposes of this sample, you can use the [party cluster](https://try.servicefabric.azure.com/).
4. Clone or download this container solution into a directory from here onwards called **[mydirectory]**

## How to Build and Deploy the container application
1. Connect to your Service Fabric cluster by running the following command

    ```sfctl cluster select --endpoint http://<my.azurecluster.name>:<port>```
2. Inside **[mydirectory]** run the following command to deploy the application: 

    ```sfctl compose create --application-id fabric:/TestContainerApp --compose-file docker-compose.yml```
3. Open a browser and browse to **http://[my.azurecluster.name]:4000** and you should see the Voting web UI. 
5. To remove the application from your cluster, run the remove command provided in the directory. Note that this command may take some time to run. 

    ```sfctl compose remove  --application-id TestContainerApp [ --timeout ]```

## More information
The [Service Fabric container documentation](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-linux-overview) provides details on the container features and scenarios.

The following are other useful links which contain more in depth information
- [Docker Compose on Service Fabric](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-docker-compose)

## MSFT OSS Code Of Conduct Notice
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

<!-- Links -->

[service-fabric-docs]: http://aka.ms/servicefabricdocs
[service-fabric-containers-overview]: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-containers-overview/
[service-fabric-samples]: http://aka.ms/servicefabricsamples
