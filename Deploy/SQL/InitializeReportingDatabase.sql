-- Create the incident reporting table

CREATE TABLE IncidentCurrent

(IncidentId INT
, IncidentStatus VARCHAR(255)
, IncidentPriority VARCHAR(255)
, PriorityNumber INT
, IncidentGroup VARCHAR(255)
, IncidentCategory VARCHAR(255)
, ResponseTime DECIMAL(5,2)
, TargetResponseTime DECIMAL(5,2)
, IncidentReceivedTime time(1)
, CallNumber VARCHAR(255)
, Description VARCHAR(255)
, District VARCHAR(255)
, Latitude VARCHAR(255)
, Longitude VARCHAR(255)
)
GO

--Create table TicketsCurrent
CREATE TABLE TicketsCurrent

(TicketId INT
, TicketStatus VARCHAR(255)
, OffenceType VARCHAR(255)
, DisputedTickets VARCHAR(255)
, ViolationType VARCHAR(255)
, PoliceOfficer VARCHAR(255)
, District VARCHAR(255)
, Latitude VARCHAR(255)
, Longitude VARCHAR(255)
, TicketNotes VARCHAR(255)
)
GO

--Create table OfficerStatus
CREATE TABLE OfficerStatus

(CurrentStatus VARCHAR(255)
, HoursOnShift INT
, ShiftLimit INT
, Officer VARCHAR(255)
)
GO