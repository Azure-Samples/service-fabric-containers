---
services: service-fabric
platforms: dotnet
author: msfussell
---

# Service Fabric Container Samples
This repository contains sample projects to help you get started with Service Fabric and Containers both on Windows and Linux.

# Windows Server 2016 Container Sample
This sample shows containers running on Windows Server 2016. The solution has a frontend service written in C# that listens for web requests on port 80 and
a backend service written in nodejs that serves up a web page with information about the node it is running on.

# Linux Container Sample
This sample shows containers running on Ubuntu 16.04. The solution has a frontend service written in NodeJS that listens to web requests on port 4000 and uses DNS resolution to request information from a backend service written in Python.

## MSFT OSS Code Of Conduct Notice
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

<!-- Links -->

[service-fabric-docs]: http://aka.ms/servicefabricdocs
[service-fabric-containers-overview]: https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-containers-overview/
[service-fabric-samples]: http://aka.ms/servicefabricsamples
