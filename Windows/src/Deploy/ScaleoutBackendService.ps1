#Connect-ServiceFabricCluster <mycluster.cloudapp.azure.com>:19000

Update-ServiceFabricService -Stateless -ServiceName fabric:/Container.Application/BackEndService -InstanceCount -1 -Force