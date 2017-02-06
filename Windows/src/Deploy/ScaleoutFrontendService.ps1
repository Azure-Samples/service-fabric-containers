#Connect-ServiceFabricCluster <mycluster.cloudapp.azure.com>:19000

Update-ServiceFabricService -Stateless -ServiceName fabric:/Container.Application/FrontEndService -InstanceCount -1 -Force