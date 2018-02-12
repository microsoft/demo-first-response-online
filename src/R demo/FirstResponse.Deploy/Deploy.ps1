. ".\Scripts\Functions\CreateResourceGroup.ps1"
. ".\Scripts\Functions\DeployTemplate.ps1"

$resourceGroupLocation =  "West Europe"
$generalTemplateFile = ".\Templates\FirstResponseR.json"

$resourceGroupName = "FirstResponse.RExperiment"

#Login into azure rm account
Login-AzureRmAccount

# Select FirstResponse R Subscription. Edit if you need another
Select-AzureRmSubscription -SubscriptionId "dd323474-a5cb-40c9-9360-3dbc04a7cf90"

# Create or update resource group
CreateResourceGroup -resourceGroupName $resourceGroupName -resourceGroupLocation $resourceGroupLocation -ErrorAction Stop

# Deploy SQL Azure account
Write-Host "Deploying SQL Azure DB account..."
$templateParameters = @{}
DeployTemplate -templateFile $generalTemplateFile -templateParametersObject $templateParameters -resourceGroupName $resourceGroupName -ErrorAction Stop