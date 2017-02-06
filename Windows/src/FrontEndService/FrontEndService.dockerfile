# ------------------------------------------------------------
#  Copyright (c) Microsoft Corporation.  All rights reserved.
#  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
# ------------------------------------------------------------

FROM microsoft/windowsservercore:10.0.14393.321
LABEL Description="Guest executable on Service Fabric in a Docker container" Vendor="self" Version="1.0"
ADD bin/debug/FrontEndService.exe /codelocation/
EXPOSE 80
ENTRYPOINT ["/codelocation/FrontEndService.exe"]