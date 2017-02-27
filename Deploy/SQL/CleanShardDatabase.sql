
--Clean the structure out.

IF EXISTS (SELECT *
           FROM   sys.objects
           WHERE  object_id = OBJECT_ID(N'[dbo].[policy_incident]'))
	DROP SECURITY POLICY policy_incident
GO

IF EXISTS (SELECT *
           FROM   sys.objects
           WHERE  object_id = OBJECT_ID(N'[Security].[fn_incident_predicate]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
  DROP FUNCTION [Security].fn_incident_predicate;
GO 


--Users
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'JohnC')
BEGIN
	DROP USER [JohnC]
END

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'EvanD')
BEGIN
	DROP USER [EvanD]
END

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'BenM')
BEGIN
	DROP USER [BenM]
END

--ROLE
IF DATABASE_PRINCIPAL_ID('Supervisor') IS NOT NULL
BEGIN
  DROP ROLE Supervisor;
END

-- Tables
IF OBJECT_ID('UserRegion', 'U') IS NOT NULL 
BEGIN
	DROP TABLE [UserRegion]
END 

IF OBJECT_ID('User', 'U') IS NOT NULL 
BEGIN
	DROP TABLE [User]
END 

IF EXISTS (SELECT * FROM sys.schemas WHERE name = 'Security')
BEGIN
	
	DROP SCHEMA [Security]
END

-- Sharded table containing our sharding key (IncidentId)
IF OBJECT_ID('Incidents', 'U') IS NOT NULL 
BEGIN
	DROP TABLE [Incidents]
END 