# FIRST RESPONSE ONLINE

### Requirements

* [Visual Studio __2015__](https://www.visualstudio.com/en-us/products/vs-2015-product-editions.aspx) (14.0 or higher) to compile C# 6 langage features  
  Ensure to check "Universal Windows Platform Tools" on installation.
* Xamarin add-ons for Visual Studio (available via the Visual Studio installer)
* __Visual Studio Community Edition is fully supported!__
* Android SDK Tools 25.2.3 or higher (available via the Visual Studio installer)
* [JDK 8.0](http://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html)
* [SQL Server 2016 Management Studio](https://msdn.microsoft.com/library/mt238290.aspx) required to Connect to SQL Database By Using Azure Active Directory Authentication

### Deployment instructions.

Before starting the deployment, make sure you have access to an Azure subscription.

1. Clone this repository in your local machine.
2. Ensure you have the latest version of PowerShell install (tested against 5.1.0 – February 2017). Up to date version can be found [here](http://aka.ms/webpi-azps)
3. From a PowerShell window run `.\Deploy\deploy.ps1` with the following parameters
  * -PrefixName (mandatory)  
    This PrefixName is used for the name of the resource group and is prefixed to all the Azure resources, which are created.
  * -ResourceGroupLocation westus (optional : default westus)  
    This is the location where the resource group is created, if not specified it will default to westus.
  * -WebConfigPath (mandatory)  
    Part of this demo relies on debugging using Visual Studio. To reduce the overhead of taking the connection strings and Azure search credentials from the Azure Portal after deployment we will programmatically replace keys created from deployment into the web.config for debugging. This is a one-time replacement, running a deployment the second time will retain the previous keys. 
To re-apply the changes to the web.confg file replace with the web.config.backup.
  * –AzureADAccount (optional)  
    The AzureADAccount is a user you have credentials for to login. This will be used to show Azure AD Authentication to SQL server. 
If this is left blank you can always update this from within the Azure portal as defined [here](https://azure.microsoft.com/en-us/documentation/articles/sql-database-aad-authentication/).

  * Select the subscription required when prompted.
4. Wait for the script to complete.  
  **Important**: note down your prefix and SQL server name which is outputted during the deployment, you will need these during the demo.  
  **Note**: The initial deployment can take ~10-15 minutes while the Azure components are provisioned. Subsequent runs to re generate data should be much quicker providing the same –PrefixName is provided.
5. You are ready to open **MSCorp.FirstResponse.sln** with Visual Studio and run the application.  
  The following credentials will be created by default:  
  Username: jclarkson, pwd: jclarkson [Attending Officer]  
  Username: edodds, pwd: edodds [Supervisor]

### Power BI

As part of the demo you could find a PowerBI report for the city of Seattle that you can deploy as a Power BI Embedded resource. To do so, follow the below instructions:

1. Install [Power BI command line tool](https://github.com/Microsoft/PowerBI-Cli)  
  `npm install powerbi-cli -g`
2. [Create a workspace collection](https://docs.microsoft.com/en-us/azure/power-bi-embedded/power-bi-embedded-get-started) and get the Access Key
3. Create a new workspaced within a workspace collection  
  `powerbi create-workspace -c <collection> -k <accessKey>`  
  Copy the WorkspaceId to use it in the next step.
4. Import the PBIX file  
  `powerbi import -c <collection> -w <workspaceId> -k <accessKey> -f "./PowerBI/Police Supervisor Dashboard.pbix" -n [name]`  
5. Get your ReportId  
  `powerbi get-reports -c <collection> -w <workspaceId> -k <accessKey>`  
  Copy the ReportId to use it in the next step.
6. Open the Web.config file located at src\API\MSCorp.FirstResponse.WebApiDemo\ and update the PowerBI appSettings in order to complete this setup.

### Clean up

To clean up the Azure assets:

1. Login to the Azure portal.
2.  Delete the resource group created as part of the deployment – It will have the same name as the prefix used as part of the deployment step. Internally this will delete all the child resources created in Azure.
3.  Uninstall the First Response Online application from the devices. 


