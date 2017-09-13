---
services: service-fabric
platforms: dotnet
author: msfussell
---

# Service Fabric Container Samples
This repository contains a sample project to help you get started with Service Fabric and Containers on Linux.

# Linux Ubuntu 16.04 Container Sample
The solution has three folders. The frontend and backend folders contain the NodeJS and Python services respectively along with their Dockerfile's. The SimpleContainerApp is the Service Fabric application that is used to deploy the containerized services.  

## Prerequisites for development machine
1. Get a physical machine or Azure VM with "Ubuntu 16.04" for your development machine. 
2. [Set up the developer environment](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started-linux) 
3. Create a Service Fabric Linux cluster on Azure with a minimum of five nodes. 
    NOTE: The Linux demo requires a cluster running on Azure. For the purposes of this sample, you can use the [party cluster](https://try.servicefabric.azure.com/).
4. Clone or download this container solution into a directory from here onwards called **[mydirectory]**

## How to Build and Deploy the container application
1. Build the solution using gradle inside **[mydirectory]**
2. Connect to your Service Fabric cluster by running the following command

    ```sfctl cluster select --endpoint http://<my.azurecluster.name>:<port>```
3. Use the install script provided in the directory to copy the application package to your cluster's image store, register the application type and create an instance of the application. 

    ```./install.sh```
4. Open a browser and browse to **http://[my.azurecluster.name]:4000** and you should see a web page with the names of the nodes the backend and frontend services are running on. 
5. To remove the application from your cluster, run the uninstall script provided in the directory. Note that this command may take some time to run. 

    ```./uninstall.sh```

## More information
The [Service Fabric container documentation](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-linux-overview) provides details on the container features and scenarios.

The following are other useful links which contain more in depth information
- [Create a Linux Container Application](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-get-started-containers-linux)

## MSFT OSS Code Of Conduct Notice
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

<!-- Links -->

[service-fabric-docs]: http://aka.ms/servicefabricdocs
[service-fabric-containers-overview]: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-containers-overview/
[service-fabric-samples]: http://aka.ms/servicefabricsamples
