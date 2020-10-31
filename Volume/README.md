---
services: service-fabric
platforms: dotnet
author: abhishekram
---
# Container volume sample
This sample illustrates the use of a volume, provided by an [Azure Files](https://docs.microsoft.com/azure/storage/files/storage-files-introduction) file share, in a Service Fabric container application. The volume is mounted to a specific location within the container. This is specified in the application manifest as shown below:

    <Volume Source="azfiles" Destination="c:\VolumeTest\Data" Driver="sfazurefile">

In the above example, the volume is mounted in the path "c:\VolumeTest\Data" in the container. The sample application that runs inside the container writes a text file under this path.

More information about specifying a volume for a container in a Service Fabric application is available [here](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-containers-volume-logging-drivers).

## Prerequisites
### Windows operating system
This sample depends on the [Azure Files volume plugin](http://download.microsoft.com/download/C/0/3/C0373AA9-DEFA-48CF-9EBE-994CA2A5FA2F/AzureFilesVolumePlugin.6.255.389.9494.zip). The Windows version of this plugin is supported only on [Windows Server version 1709](https://docs.microsoft.com/en-us/windows-server/get-started/whats-new-in-windows-server-1709), [Windows 10 version 1709](https://docs.microsoft.com/en-us/windows/whats-new/whats-new-windows-10-version-1709) or later operating systems. Therefore this sample also works only on those operating systems.

_Note:_ For Linux, the Azure Files volume plugin - and hence this sample too - works on all operating system versions supported by Service Fabric.

### Azure Files volume plugin
Deploy the Azure Files volume plugin to the cluster by following the instructions in its [documentation](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-containers-volume-logging-drivers#deploy-the-service-fabric-azure-files-application).

### Create the Azure Files file share
Follow the instructions in the [Azure Files documentation](https://docs.microsoft.com/en-us/azure/storage/files/storage-how-to-create-file-share) to create a file share for the application to use.

## Build the application and Docker container image
The application can be built by running the _src\build.Windows.cmd_ script on Windows and the _src/build.Linux.sh_ script on Linux. Running these scripts results in the creation of the _src\pkg\Windows_ and _src/pkg/Linux_ folders which contain the application binaries for Windows and Linux respectively. They also contain the _dockerfile_ that can be used to build the Docker container image for the application.

After building the application, build the Docker container image and push it to a Docker repository. An example for building a Docker container image and pushing it to a repository can be found [here](https://github.com/Azure-Samples/service-fabric-containers/tree/master/Windows#how-to-build-and-deploy-the-containerapplication). When building the Docker container image for Windows, use version 1709 or later of Windows, as per the operating system requirements mentioned [above](#windows-operating-system).

## Deploy the sample application
The sample application packages for Windows and Linux can be found in the _Application\Windows_ and _Application/Linux_ folders respectively. The application can be deployed to the cluster via [PowerShell](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-deploy-remove-applications), [CLI](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-application-lifecycle-sfctl) or [FabricClient APIs](https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-deploy-remove-applications-fabricclient).

As an example, the steps for deploying the sample application via PowerShell are shown below.

### 1. Update the application package
#### Container image information
In ServiceManifest.xml, provide the path to the container image in the Docker repository. For more information, please see [above](#build-the-application-and-docker-container-image).

    <ImageName></ImageName>

In ApplicationManifest.xml, provide the account name and password for the repository where the Docker image is present. If the repository is public and does not require a user name and password to access, then remove the line below from the application manifest.

    <RepositoryCredentials AccountName="" Password="" />

#### Azure Files file share information
In ApplicationManifest.xml, provide the storage account name, storage account key and file share name for the Azure Files file share that provides the volume for the container.

    <DriverOption Name="shareName" Value="" />
    <DriverOption Name="storageAccountName" Value="" />
    <DriverOption Name="storageAccountKey" Value="" />

### 2. Copy the application package to the image store
Run the command below with the appropriate value for [ApplicationPackagePath] and [ImageStoreConnectionString]:

    Copy-ServiceFabricApplicationPackage -ApplicationPackagePath [ApplicationPackagePath] -ImageStoreConnectionString [ImageStoreConnectionString] -ApplicationPackagePathInImageStore VolumeTest

### 3. Register the application type

    Register-ServiceFabricApplicationType -ApplicationPathInImageStore VolumeTest

### 4. Create the application

    New-ServiceFabricApplication -ApplicationName fabric:/VolumeTestApp -ApplicationTypeName VolumeTestAppType -ApplicationTypeVersion 1.0.0

## Verify that the application is able to use the volume
As mentioned [earlier](#container-volume-sample), the sample application creates a file named _data.txt_ in the file share (if it does not exist already). The content of this file is a number that is incremented every 30 seconds by the application. To verify that the example works correctly, open the _data.txt_ file periodically and verify that the number is being updated.

The file may be downloaded using any tool that enables browsing an Azure Files file share. The [Microsoft Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/) is an example of such a tool.

## Docker Hub rate limits

Effective November 2, 2020, download rate limits apply to anonymous and authenticated requests to Docker Hub from Docker Free plan accounts and are enforced by IP address.

This sample pulls the following public images from Docker Hub. Please note that you may be rate limited.

| Source | Image   |
| -------------|-------------|
| src\VolumeTestApp.dockerfile | microsoft/dotnet:2.0-runtime |

For more details, see [Authenticate with Docker Hub](https://docs.microsoft.com/en-us/azure/container-registry/buffer-gate-public-content#authenticate-with-docker-hub).
