#Shard map manager (Run One step at a time, follow the sequence) -> use F8 to run selection

#Step 1-Import the shardmapmanagement modules
Import-Module .\ShardManagement.psm1

#Step 2- Get ShardMapManager (to list the available shards which are online)

$mgr = Get-ShardMapManager -UserName [YOUR_PREFIX]-admin -Password f1r4tR3sp0Ns1 -SqlServerName [YOUR_PREFIX]-sqlserver.database.windows.net -SqlDatabaseName [YOUR_PREFIX]-master
$map = Get-RangeShardMap -ShardMapManager $mgr -RangeShardMapName 'IncidentShardMap' -KeyType int 
$map.GetMappings() | Format-List

#Step 3- Show the cmdlets and functions
Get-Command -Module ShardManagement

# In order to get the incidents list, you need to provide the cityId parameter
# modify {cityId} in the below URLs for your desired city
# List of IDs are
# 24 | Chicago
# 25 | Johannesburg
# 26 | Frankfurt
# 27 | Washington, DC
# 28 | Singapore
# 29 | Bangalore
# 30 | Milan
# 31 | Amsterdam
# 32 | Birmingham
# 33 | Copenhagen
# 34 | Seoul
# 35 | Seattle

#Step 4
Invoke-RestMethod http://localhost:50002/api/incidents/{cityId} -TimeoutSec 9999 | ft

#Step 5- Optional (same as above)
Invoke-RestMethod http://localhost:50002/api/incidents/ambulance/{cityId} -TimeoutSec 9999

#Step 6- Search
$searchResults = Invoke-RestMethod http://localhost:50002/api/person/search?searchText=Joe -TimeoutSec 9999
$searchResults.Results
$searchResults.Facets