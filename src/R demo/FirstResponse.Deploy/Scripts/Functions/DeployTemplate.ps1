function DeployTemplate
{
	Param(
	 [Parameter(Mandatory=$true)]
	 [string]$templateFile,
	 [Parameter(Mandatory=$true)]
	 [object]$templateParametersObject,
	 [Parameter(Mandatory=$true)]
	 [string]$resourceGroupName
	)

	Write-Host "Deploying ARM template: " $templateFile
	$result = New-AzureRmResourceGroupDeployment -Name ((Get-ChildItem $templateFile).BaseName + '-' + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')) `
										   -ResourceGroupName $resourceGroupName `
										   -TemplateFile $templateFile `
										   -TemplateParameterObject $templateParametersObject `
										   -Force -Verbose `
										   -ErrorVariable ErrorMessages `

	$ErrorMessages = $ErrorMessages | ForEach-Object { $_.Exception.Message.TrimEnd("`r`n") }

	if ($ErrorMessages)
	{
		"", ("{0} returned the following errors:" -f ("Template deployment", "Validation")[[bool]$ValidateOnly]), @($ErrorMessages) | ForEach-Object { Write-Output $_ }
	}

	return $result
}