<# 
Note: Replace X with the demo prefix used at database creation time. These can be also be retrieved from Azure portal or the demo application web.config file.
Note: Replace Y with the folder to the deployment package.
#>
Import-Module Y/deployment/ShardManagement.ps1 -Verbsose

$mgr = Get-ShardMapManager 
    -UserName X-admin
    -Password f1r4tR3sp0Ns1 
    -SqlServerName 
     X-sqlserver.database.windows.net
    -SqlDatabaseName X-master

$map = Get-RangeShardMap
    -ShardMapManager $mgr
    -RangeShardMapName
    'IncidentShardMap'
    -KeyType int

$map.GetMappings() | Format-List

Get-Command -Module ShardManagement
