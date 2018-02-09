function CreateResourceGroup
{
	Param(
	 [Parameter(Mandatory=$true)]
	 [string]$resourceGroupName,
	 [Parameter(Mandatory=$true)]
	 [string]$resourceGroupLocation
	)

	Write-Host "Creating or updating resource group: "$resourceGroupName

	# Create or update resource group
	New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation -Verbose -Force -ErrorAction Stop
}