CREATE TABLE [dbo].[DataSet] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ZoneId]           INT            NOT NULL,
    [Distance1000]     INT            NOT NULL,
    [Distance3000]     INT            NOT NULL,
    [Distance6000]     INT            NOT NULL,
    [DistancePlus6000] INT            NOT NULL,
    [Temperature]      INT            NOT NULL,
    [SeaLvlPress]      INT            NOT NULL,
    [Windspeed]        INT            NOT NULL,
    [Rain]             DECIMAL (5, 2) NOT NULL,
    [EventCount]       INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

