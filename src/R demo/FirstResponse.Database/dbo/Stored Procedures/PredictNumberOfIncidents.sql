CREATE PROCEDURE [dbo].[PredictNumberOfIncidents] 
@distance1000 INT, @distance3000 INT, @distance6000 INT, @distance6000Plus INT, @temperature INT, @pressure INT, @windspeed INT, @rain DECIMAL AS
BEGIN
	DECLARE @serialized VARBINARY(MAX) = (SELECT TOP 1 Serialized FROM Model)
	DECLARE @s NVARCHAR(MAX) = N'
	incidence = InputDataSet
	model = unserialize(as.raw(m))
	p = rxPredict(model, incidence, type = "response")[[1]]
	OutputDataSet = data.frame(p)'

	DECLARE @q NVARCHAR(MAX) = CONCAT('SELECT ',@distance1000,' AS Distance1000, ',@distance3000, ' AS Distance3000,', @distance6000,' AS Distance6000, ', @distance6000Plus,' AS DistancePlus6000,',@temperature,' AS Temperature,', @pressure,' AS SeaLvlPress,', @windspeed,' AS Windspeed,', @rain,' AS Rain')
	
	EXEC sp_execute_external_script @language = N'R', @script = @s, @input_data_1 = @q, @params = N'@m VARBINARY(MAX)', @m = @serialized
		WITH RESULT SETS ((Predicted FLOAT))
END