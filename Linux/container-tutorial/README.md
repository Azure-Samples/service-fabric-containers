# Service Fabric Container Samples
This repository contains a sample project to help you get started with Service Fabric and Containers on Linux.

# Linux Ubuntu 16.04 Container Sample
The solution contains two folders and a 'docker-compose.yml' file. The 'azure-vote' folder contains the Python frontend service along with the Dockerfile used to build the image. The 'Voting' directory contains the Service Fabric application package that is deployed to the cluster. Service Fabric clusters require applications to follow an application package structure and the 'Voting' directory defines that structure for this project. Alternatively, to deploy a container based solution to Service Fabric, Docker compose can be used and the 'docker-compose.yml' is the compose file used to define this project. 

## Instructions to Deploy this Project
Please follow the instructions on the [Service Fabric Quickstart](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-quickstart-containers-linux) to deploy this project to a Service Fabric cluster.

## More information
The [Service Fabric container tutorial](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-tutorial-create-container-images) provides a more in depth walk through of how to build, package and deploy this project to a Service Fabric cluster. 

The following are other useful links which contain more in depth information
- [Docker Compose on Service Fabric](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-docker-compose)

## MSFT OSS Code Of Conduct Notice
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

<!-- Links -->

[service-fabric-docs]: http://aka.ms/servicefabricdocs
[service-fabric-containers-overview]: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-containers-overview/
[service-fabric-samples]: http://aka.ms/servicefabricsamples
