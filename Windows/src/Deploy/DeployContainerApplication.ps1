#Connect-ServiceFabricCluster <mycluster.cloudapp.azure.com>:19000

New-ServiceFabricApplication `
    -ApplicationTypeName "Container.ApplicationType" `
    -ApplicationTypeVersion "1.0.0" `
    -ApplicationName "fabric:/Container.Application"
    
New-ServiceFabricService `
    -Stateless `
    -PartitionSchemeSingleton `
    -ApplicationName "fabric:/Container.Application" `
    -ServiceTypeName "FrontEndServiceType" `
    -ServiceName "fabric:/Container.Application/FrontEndService" `
    -InstanceCount 1

New-ServiceFabricService `
    -Stateless `
    -PartitionSchemeSingleton `
    -ApplicationName "fabric:/Container.Application" `
    -ServiceTypeName "BackEndServiceType" `
    -ServiceName "fabric:/Container.Application/BackEndService" `
    -InstanceCount 1

