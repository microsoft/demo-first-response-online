-- 1. See that John has two Region mappings
SELECT		U.UserName, UR.RegionId
FROM		dbo.[UserRegion] UR
INNER JOIN	dbo.[User] U 
ON			UR.UserId = U.UserId
WHERE		U.UserName = 'JohnC'

-- 2. Only incidents returned from (98074, 98075)
EXECUTE ('SELECT * FROM dbo.Incidents') AS USER = 'JohnC' 

-- 3. See that Ben has two Region mappings
SELECT		U.UserName, UR.RegionId
FROM		dbo.[UserRegion] UR
INNER JOIN	dbo.[User] U 
ON			UR.UserId = U.UserId
WHERE		U.UserName = 'BenM'

-- 4. Only incidents returned from (98053, 98052)
EXECUTE ('SELECT * FROM dbo.Incidents') AS USER = 'BenM' 

-- 5. See that EvanD does not have any regions mapped.
SELECT		U.UserName, UR.RegionId
FROM		dbo.[UserRegion] UR
INNER JOIN	dbo.[User] U 
ON			UR.UserId = U.UserId
WHERE		U.UserName = 'EvanD'

-- 6. But is in the supervisor role
SELECT is_Rolemember ( 'Supervisor', 'EvanD' ) as IsInSupervisorRole

-- 7. So can see all incidents.
EXECUTE ('SELECT * FROM dbo.Incidents') AS USER = 'EvanD'

-- 8. dbo / admin role does cannot see incidents either.
SELECT * FROM Incidents


