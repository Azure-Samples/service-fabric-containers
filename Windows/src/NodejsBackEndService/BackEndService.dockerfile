# ------------------------------------------------------------
#  Copyright (c) Microsoft Corporation.  All rights reserved.
#  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
# ------------------------------------------------------------

FROM microsoft/windowsservercore:latest
LABEL Description="Guest executable on Service Fabric in a Docker container" Vendor="self" Version="1.0"
ADD sources /build
ADD BackEndService.js /jslocation/
ADD index.html /jslocation/
RUN msiexec /i C:\build\node-v6.9.1-x64.msi /qn /l*v C:\build\node-v6.9.1-x64.msi.log & del C:\build\node-v6.9.1-x64.msi
EXPOSE 8905
ENTRYPOINT ["node.exe", "/jslocation/BackEndService.js"]