--------------------------------------------------------------
-- SETUP User and User groups and roles.
--------------------------------------------------------------

CREATE USER JohnC WITHOUT LOGIN;
CREATE USER EvanD WITHOUT LOGIN; 
CREATE USER BenM WITHOUT LOGIN; 

GRANT SELECT ON dbo.[Incidents] to EvanD
GRANT SELECT ON dbo.[Incidents] to JohnC
GRANT SELECT ON dbo.[Incidents] to BenM

CREATE ROLE Supervisor;
EXEC sp_addrolemember 'Supervisor', 'EvanD';


CREATE TABLE [User] (
	[UserId]			INT				NOT NULL, 
	[UserName]			sysname			NOT NULL -- name of the user 
 CONSTRAINT [PK_User_UserId] PRIMARY KEY CLUSTERED ([UserId] ASC)
) 
GO

CREATE TABLE [UserCity] (
	[UserId]			INT				NOT NULL, 
	[CityId]			INT				NOT NULL

 CONSTRAINT [PK_UserCity] PRIMARY KEY CLUSTERED ([UserId] ASC, [CityId] ASC),
 CONSTRAINT [FK_User] FOREIGN KEY (UserID) REFERENCES [User](UserId)
) 
GO

INSERT INTO [User] VALUES
(1, 'edodds'),
(2, 'jclarkson'),
(3, 'bmartens'),
(4, 'dbo'),
(5, 'EvanD'),
(6, 'JohnC'),
(7, 'BenM')

GO

INSERT INTO [UserCity] VALUES 
(1, 25),
(1, 24),
(3, 24),
(3, 25),
(3, 26),
(3, 27),
(3, 28),
(3, 29),
(3, 30),
(3, 31),
(3, 32),
(3, 33),
(3, 34),
(6, 25),
(6, 24),
(7, 26),
(7, 27)
GO 


