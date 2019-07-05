---
languages: []
products:
- azure
topic: sample
---

# Service Fabric Container Samples
This repository contains sample projects to help you get started with Service Fabric and Containers both on Windows and Linux.

# Windows Server 2016 Container Sample
This sample shows containers running on Windows Server 2016. The solution has a frontend service written in C# that listens for web requests on port 80 and
a backend service written in nodejs that serves up a web page with information about the node it is running on.

# Linux Container Sample
This folder contains two samples showing containers running on Ubuntu 16.04. 

The 'container-dns-sample' shows multiple containers running in Service Fabric communicating over the DNS portal. It has a frontend service written in NodeJS that listens to web requests and uses DNS resolution to request information from a backend service written in Python. The containers are built using Dockerfiles and the Service Fabric application is deployed using the traditional Service Fabric Application Package structure. 

The 'container-tutorial' has a frontend service in Python which communicates with a Redis data store to render a Voting UI to the user. In this example, deployments of applications using docker-compose and Service Fabric Command Line Interface are demonstrated. 

## MSFT OSS Code Of Conduct Notice
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

<!-- Links -->

[service-fabric-docs]: http://aka.ms/servicefabricdocs
[service-fabric-containers-overview]: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-containers-overview/
[service-fabric-samples]: http://aka.ms/servicefabricsamples