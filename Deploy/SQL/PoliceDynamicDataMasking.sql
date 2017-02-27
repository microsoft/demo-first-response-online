--Add masking to Phone Number, showing the first 5 followed by masked phone number.
ALTER TABLE dbo.[Incidents]
ALTER COLUMN Phone ADD MASKED WITH (FUNCTION = 'partial(5," XXX-XXXX",0)');

--Allow users in supervisor role to see unmasked fields.
GRANT UNMASK TO Supervisor;

--Remove the masking permission from Supervisor
--REVOKE UNMASK FROM Supervisor;