IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_clearschemaversion')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_clearschemaversion] 
	AS
	BEGIN
		DELETE FROM dbo.SchemaVersions
	END')
END