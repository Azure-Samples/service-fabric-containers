#Connect-ServiceFabricCluster <mycluster.cloudapp.azure.com>:19000

Get-ServiceFabricApplication | Remove-ServiceFabricApplication -Force
Get-ServiceFabricApplicationType | Unregister-ServiceFabricApplicationType -Force