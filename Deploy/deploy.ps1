param (
    [Parameter(Mandatory = $true, HelpMessage = 'The prefix name will be used to construct URLs and names for deployed Azure components. Should be globally unique.')]
    [string] $PrefixName = "",
	
    [Parameter(Mandatory = $true, HelpMessage = 'Web.Config to update')]
    [string] $WebConfigPath = "",
	
    [Parameter(Mandatory = $false, HelpMessage = 'The location the resource group will be created in if it doesnt already exist.')]
    [string] $ResourceGroupLocation = "West US",
	
	[Parameter(Mandatory = $false, HelpMessage = 'Azure AD Account to make a admin of the SQL database')]
    [string] $AzureADAccount = ""
)

#------------------#
# Load Environment #
#------------------#

Write-Host ...............................
Write-Host 'Preparing environment'
Write-Host ...............................
Write-Host

$script_dir = Split-Path $MyInvocation.MyCommand.Path
Write-Host 'Script dir :' $script_dir

Write-Host 'Unblocking files at :' $script_dir
gci -Recurse $script_dir | Unblock-File

Write-Host 'Importing utils'
#Import utilities
. "$script_dir\deploy_utils.ps1"
. "$script_dir\sql_invoke.ps1"

Remove-Module MSCorp.FirstResponse.AzureSearch.Commands -ErrorAction Ignore
Remove-Module MSCorp.DocumentDb.Commands -ErrorAction Ignore

Import-Module $script_dir\ShardManagement.psm1
Import-Module $script_dir\DocumentDb\MSCorp.DocumentDb.Commands.dll
Import-Module $script_dir\AzureSearch\Commands\MSCorp.FirstResponse.AzureSearch.Commands.dll
Import-Module AzureRM.Profile
Import-Module AzureRM.Resources
Import-Module Azure

#------------------------------#
# Ensure the user is signed in #
#------------------------------#

Write-Host ...............................
Write-Host 'Verifying authentication'
Write-Host ...............................
Write-Host



$rm_context = 
    try {
        Get-AzureRmContext
    } 
    catch {
        if ($_.Exception -and $_.Exception.Message.Contains('Login-AzureRmAccount')) { $null } else { throw }
    }

if (-not $rm_context) {
    $title = 'You must sign in with your Azure account to continue'
    $message = 'Sign in?'
    
    if ((Confirm-Host -Title $title -Message $message) -eq 1) {
        # User declined
        return
    }
    
    $rm_context = Add-AzureRmAccount
}

if (-not $rm_context) {
    Write-Warning 'Unable to sign in?'
    return
}

#-----------------------#
# Select a subscription #
#-----------------------#

Write-Host ...............................
Write-Host 'Selecting subscription'
Write-Host ...............................
Write-Host


$azure_subscriptions = Get-AzureRmSubscription
$rm_subscription_id = $null;

if ($azure_subscriptions.Count -eq 1) {
    $rm_subscription_id = $azure_subscriptions[0].SubscriptionId
    Write-Host 'Selected single ' $rm_subscription_id
} elseif ($azure_subscriptions.Count -gt 0) {
    # Build an array of bare subscription IDs for lookups
    $subscription_ids = $azure_subscriptions | % { $_.SubscriptionId }
        
    Write-Host 'Available subscriptions:'
    $azure_subscriptions | Format-Table SubscriptionId,SubscriptionName -AutoSize
    
    # Loop until the user selects a valid subscription Id
    while (-not $rm_subscription_id -or -not $subscription_ids -contains $rm_subscription_id) {
        $rm_subscription_id = Read-Host 'Please select a valid SubscriptionId from list'
    }

    Write-Host 'Selected subscription ' $rm_subscription_id
}

if (-not $rm_subscription_id) {
    Write-Warning 'No subscription available'
    return
}

Select-AzureRmSubscription -SubscriptionId $rm_subscription_id | out-null

$rm_resource_group_name = $PrefixName

#---------------------------------#
# Start resource group deployment #
#---------------------------------#

Write-Host ...............................
Write-Host "Acquiring resource group ($rm_resource_group_name)"
Write-Host ...............................
Write-Host

$rm_resource_group = try { Get-AzureRmResourceGroup -Name $rm_resource_group_name -Verbose }
                     catch { if ($_.Exception.Message -eq 'Provided resource group does not exist.') { $null } else { throw } }

if (-not $rm_resource_group) {
    Write-Host "Creating resource group $rm_resource_group_name..."
    $rm_resource_group = New-AzureRmResourceGroup  -Name $rm_resource_group_name -Location $ResourceGroupLocation -Verbose
}

if (-not $rm_resource_group) {    
    Write-Warning 'No resource group!'
    return
}

Write-Host ...............................
Write-Host 'Starting resource group deployment'
Write-Host ...............................
Write-Host


$template = "$(Split-Path $MyInvocation.MyCommand.Path)\deployment.json"

$params = @{
    'prefix_name' = $PrefixName;
}

$result = New-AzureRmResourceGroupDeployment -ResourceGroupName $rm_resource_group_name -TemplateFile $template -TemplateParameterObject $params -Verbose

if (-not $result -or $result.ProvisioningState -ne 'Succeeded') {
    Write-Warning 'An error occured during provisioning'
    Write-Output $result
    return
}
	
#-----------------------#
# Store outputs	for use #
#-----------------------#
$OUTPUTS = @{}
foreach ($name in $result.Outputs.Keys) {
	$OUTPUTS[$name] = $result.Outputs[$name].Value
}

# Write-Host "DocumentDb Data";
# Write-Host  $OUTPUTS['DocumentDbUri'];
# Write-Host $OUTPUTS['DocumentDbAuthKey'];


#----------------#
# Report results #
#----------------#
foreach ($name in $result.Outputs.Keys) {
    $keyAndValue = $name + " : " +  $result.Outputs[$name].Value
    Write-Host $keyAndValue
}

#--------------------------#
# Seed documentDb database #
#--------------------------#
Write-Host ...............................
Write-Host 'Seeding document DB data'
Write-Host ...............................
Write-Host
Add-DocumentDbSeedData -Url $OUTPUTS['DocumentDbUri'] -Key $OUTPUTS['DocumentDbAuthKey'] -DataPath $script_dir\DocumentDb\TicketData.json

#-------------------------------#
# Trasform database credentials #
#-------------------------------#
$server = $OUTPUTS['sqlServer'];
$u = $OUTPUTS['sqlUsername'];
$p = $OUTPUTS['sqlPassword'];

if (!$server.EndsWith('.database.windows.net')) {
    Write-Host 'Appending end to server name' -foregroundcolor yellow
    $server = $server + '.database.windows.net'
    Write-Host 'New server name ' $server -foregroundcolor yellow
}

if (!$u.EndsWith($server)) {
    Write-Host 'Appending Server name to user name' -foregroundcolor yellow
    $u = $u + '@'+ ($server -Replace '.database.windows.net', '')
    Write-Host 'New user name ' $u -foregroundcolor yellow
}

#---------------------------#
# Deploy / create databases #
#---------------------------#
Write-Host ...............................
Write-Host 'Cleaning Databases'
Write-Host ...............................
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlAmboDatabase'] -file "$script_dir\SQL\CleanShardDatabase.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlFireDatabase'] -file "$script_dir\SQL\CleanShardDatabase.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlPoliceDatabase'] -file "$script_dir\SQL\CleanShardDatabase.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlReportingDatabase'] -file "$script_dir\SQL\CleanReportingDatabase.sql" -u $u -p $p

Write-Host ...............................
Write-Host 'Deploying databases'
Write-Host ...............................
Write-Host
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlAmboDatabase'] -file "$script_dir\SQL\InitializeShardDatabase.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlFireDatabase'] -file "$script_dir\SQL\InitializeShardDatabase.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlPoliceDatabase'] -file "$script_dir\SQL\InitializeShardDatabase.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlReportingDatabase'] -file "$script_dir\SQL\InitializeReportingDatabase.sql" -u $u -p $p

#---------------------------#
# Populate databases 		#
#---------------------------#
Write-Host ...............................
Write-Host 'Populating Databases'
Write-Host ...............................
Write-Host
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlAmboDatabase'] -file "$script_dir\SQL\AmbulanceIncidents.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlFireDatabase'] -file "$script_dir\SQL\FireIncidents.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlPoliceDatabase'] -file "$script_dir\SQL\PoliceIncidents.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlPoliceDatabase'] -file "$script_dir\SQL\PoliceUserData.sql" -u $u -p $p
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlReportingDatabase'] -file "$script_dir\SQL\ReportingDatabase.sql" -u $u -p $p

Write-Host 'Setting up RLS'
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlPoliceDatabase'] -file "$script_dir\SQL\PoliceRowLevelSecurity.sql" -u $u -p $p
Write-Host 'Setting up DDM'
Invoke-SQL -Server $server -dbname $OUTPUTS['sqlPoliceDatabase'] -file "$script_dir\SQL\PoliceDynamicDataMasking.sql" -u $u -p $p

#---------------------------#
# Move databases into pool	#
#---------------------------#
Write-Host ...............................
Write-Host 'Move databases into pool'
Write-Host ...............................
Write-Host
Set-AzureRmSqlDatabase -ResourceGroupName $rm_resource_group_name -ServerName $OUTPUTS['sqlServer'] -DatabaseName $OUTPUTS['sqlAmboDatabase']    -ElasticPoolName $OUTPUTS['sqlElasticPool']
Write-Host $OUTPUTS['sqlAmboDatabase'] 'added to' $OUTPUTS['sqlElasticPool']
Set-AzureRmSqlDatabase -ResourceGroupName $rm_resource_group_name -ServerName $OUTPUTS['sqlServer'] -DatabaseName $OUTPUTS['sqlFireDatabase']    -ElasticPoolName $OUTPUTS['sqlElasticPool']
Write-Host $OUTPUTS['sqlFireDatabase'] 'added to' $OUTPUTS['sqlElasticPool']
Set-AzureRmSqlDatabase -ResourceGroupName $rm_resource_group_name -ServerName $OUTPUTS['sqlServer'] -DatabaseName $OUTPUTS['sqlPoliceDatabase']  -ElasticPoolName $OUTPUTS['sqlElasticPool']
Write-Host $OUTPUTS['sqlPoliceDatabase'] 'added to' $OUTPUTS['sqlElasticPool']
Set-AzureRmSqlDatabase -ResourceGroupName $rm_resource_group_name -ServerName $OUTPUTS['sqlServer'] -DatabaseName $OUTPUTS['sqlMasterDatabase']  -ElasticPoolName $OUTPUTS['sqlElasticPool']
Write-Host $OUTPUTS['sqlMasterDatabase'] 'added to' $OUTPUTS['sqlElasticPool']
Set-AzureRmSqlDatabase -ResourceGroupName $rm_resource_group_name -ServerName $OUTPUTS['sqlServer'] -DatabaseName $OUTPUTS['sqlReportingDatabase']  -ElasticPoolName $OUTPUTS['sqlElasticPool']
Write-Host $OUTPUTS['sqlReportingDatabase'] 'added to' $OUTPUTS['sqlElasticPool']
#---------------------------#
# Create Shard database		#
#---------------------------#

#Add shard map name to the output (Used here and below in the web.config replace)
$shardMapName = 'IncidentShardMap';
$ShardMapManager = Get-ShardMapManager -UserName  $u -Password $p -SqlServerName $server -SqlDatabaseName $OUTPUTS['sqlMasterDatabase']
if (!$ShardMapManager) {
    Write-Host 'Creating Shard manager database'
    $ShardMapManager = New-ShardMapManager -UserName $u -Password $p -SqlServerName $server -SqlDatabaseName $OUTPUTS['sqlMasterDatabase']

    Write-Host 'Add shard map'
    $ShardMap = New-RangeShardMap -KeyType $([int]) -RangeShardMapName $shardMapName -ShardMapManager $ShardMapManager

    Write-Host 'Add a new shards to hold the range being added'
    Add-Shard -ShardMap $ShardMap -SqlServerName $server -SqlDatabaseName $OUTPUTS['sqlAmboDatabase']
    Add-Shard -ShardMap $ShardMap -SqlServerName $server -SqlDatabaseName $OUTPUTS['sqlFireDatabase']
    Add-Shard -ShardMap $ShardMap -SqlServerName $server -SqlDatabaseName $OUTPUTS['sqlPoliceDatabase']

    Write-Host 'Create the mappings and associate it with the new shards'
    Add-RangeMapping -KeyType $([int]) -RangeHigh '1' -RangeLow '0' -RangeShardMap $ShardMap -SqlServerName $server -SqlDatabaseName $OUTPUTS['sqlAmboDatabase']
    Add-RangeMapping -KeyType $([int]) -RangeHigh '2' -RangeLow '1' -RangeShardMap $ShardMap -SqlServerName $server -SqlDatabaseName $OUTPUTS['sqlFireDatabase']
    Add-RangeMapping -KeyType $([int]) -RangeHigh '3' -RangeLow '2' -RangeShardMap $ShardMap -SqlServerName $server -SqlDatabaseName $OUTPUTS['sqlPoliceDatabase']
} else {
    Write-Host 'Shard manager already exists'
}

#------------------------------------#
# Assign Azure AD Account #
#------------------------------------#
if (-not $AzureADAccount -and $AzureADAccount -ne '') {
	Write-Host 'Adding Azure AD Account' $AzureADAccount ' as Admin to server' $OUTPUTS['sqlServer'] 'in resource group ' $rm_resource_group_name
	Set-AzureRmSqlServerActiveDirectoryAdministrator –ResourceGroupName $rm_resource_group_name –ServerName $OUTPUTS['sqlServer'] -DisplayName $AzureADAccount
}

#------------------------------------#
# Create and seed azure search index #
#------------------------------------#

$sin = $OUTPUTS['searchIndexName'];
$sak = $OUTPUTS['searchApiKey'];
$ssu = $OUTPUTS['searchServiceUrl'];
$indexCreate = "$script_dir\AzureSearch\PersonIndex.json";
$indexPopulate = "$script_dir\AzureSearch\PersonData.json";

Write-Host ...............................
Write-Host 'Creating azure search index'
Write-Host ...............................
Write-Host
Add-AzureSearchIndex -SearchIndexName $sin -SearchServiceApiKey $sak -IndexCreationJsonFile $indexCreate -SearchServiceUri $ssu -Verbose

Write-Host ...............................
Write-Host 'Populating azure search index'
Write-Host ...............................
Write-Host
Add-AzureSearchIndexSeedData -SearchIndexName $sin -SearchServiceApiKey $sak -PersonDataJsonFile $indexPopulate -SearchServiceUri $ssu -Verbose

#----------------------------------------------------#
# Update web api web config #
#----------------------------------------------------#
if ($WebConfigPath -ne '') {
	if (Test-Path $WebConfigPath){
        try {
            $webconfigContent = (Get-Content $WebConfigPath | Out-String);
		    $webconfigContent = $webconfigContent.replace('[sqlServer]', $server);
		    $webconfigContent = $webconfigContent.replace('[sqlUsername]', $OUTPUTS['sqlUsername']);
		    $webconfigContent = $webconfigContent.replace('[sqlPassword]', $OUTPUTS['sqlPassword']);
		    $webconfigContent = $webconfigContent.replace('[sqlMasterDatabase]', $OUTPUTS['sqlMasterDatabase']);
		    $webconfigContent = $webconfigContent.replace('[shardMapName]', $shardMapName);
		    $webconfigContent = $webconfigContent.replace('[searchIndexName]', $OUTPUTS['searchIndexName']);
		    $webconfigContent = $webconfigContent.replace('[searchServiceName]', $OUTPUTS['SearchServiceName']);
		    $webconfigContent = $webconfigContent.replace('[searchServiceApiKey]', $OUTPUTS['searchApiKey']);        
            $webconfigContent = $webconfigContent.replace('[DocumentDbUri]', $OUTPUTS['DocumentDbUri']);
            $webconfigContent = $webconfigContent.replace('[DocumentDbAuthKey]', $OUTPUTS['DocumentDbAuthKey']);
		    Set-Content -Path $WebConfigPath -Value $webconfigContent;
        }
        catch {
            Write-Error "Error updating file " $WebConfigPath
        }
	} else {
        Write-Error "Invalid path " $WebConfigPath
    }
} else {
    Write-Error 'Provided value for $WebConfigPath not valid'
}

Write-Host 'Deployment completed'