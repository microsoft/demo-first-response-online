CREATE PROCEDURE [dbo].[TrainIncidencesModel] AS
BEGIN
	DECLARE @q NVARCHAR(MAX) = N'SELECT * FROM dbo.Dataset'
	DECLARE @s NVARCHAR(MAX) = N'
		incidences = InputDataSet
		model <- rxDForest(EventCount ~ Distance1000 + Distance3000 + Distance6000 + DistancePlus6000 + Temperature + SeaLvlPress + Windspeed + Rain, data = incidences, maxDepth = 14, nTree = 14, cp = 0.01)
		serialized = data.frame(model = as.raw(serialize(model, NULL)))'
	BEGIN TRY
		BEGIN TRANSACTION
		DELETE FROM Model
		INSERT INTO Model
		EXEC sp_execute_external_script @language = N'R', @script = @s, @input_data_1 = @q, @output_data_1_name = N'serialized'
		COMMIT
    END TRY
	BEGIN CATCH
		PRINT 'Failed to recompute model, rolling back to previous trained model'
		ROLLBACK
	END CATCH
END