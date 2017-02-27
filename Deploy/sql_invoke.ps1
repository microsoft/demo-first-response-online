function Invoke-SQL {

	param( 
			#Name of SQL Server 
			[parameter(Mandatory=$true, 
				   HelpMessage="Specify the SQL Server name where will be run a T-SQL code",Position=0)] 
			[String] 
			[ValidateNotNullOrEmpty()] 
			$server = $(throw "sqlserver parameter is required."), 
	 
			#Database name for execution context 
			[parameter(Mandatory=$true, 
				   HelpMessage="Specify the context database name",Position=1)]
			[String] 
			[ValidateNotNullOrEmpty()] 
			$dbname = $(throw "dbname parameter is required."), 
	 
			#Name of T-SQL file (.sql) 
			[parameter(Mandatory=$true,Position=2)] 
			[String] 
			[ValidateNotNullOrEmpty()] 
			$file = $(throw "filename parameter is required."), 
			  
			#MS SQL Server user name 
			[parameter(Mandatory=$true,Position=4)] 
			[String] 
			[ValidateNotNullOrEmpty()] 
			$u = $(throw "username parameter is required."), 
	 
			#MS SQL Server password name 
			[parameter(Mandatory=$true,Position=5)] 
			[String] 
			[ValidateNotNullOrEmpty()] 
			$p = $(throw "password parameter is required.") 
		) 

Import-Module Azure

#------------------#
# Run script 	   #
#------------------#


$SQLCommandText = [system.io.file]::ReadAllText((Resolve-Path $file).ProviderPath)

$params = @{
    'Database' = $dbname
    'ServerInstance' = $server
    'Username' = $u
    'Password' = $p 
    'Query' = $SQLCommandText
	'OutputSqlErrors' = $True
	'Querytimeout' = 60
}

Write-Host 'DB: ' $dbname -foregroundcolor yellow
Write-Host 'Server: ' $server -foregroundcolor yellow
Write-Host 'User: ' $u -foregroundcolor yellow

Invoke-Sqlcmd @params
	
}