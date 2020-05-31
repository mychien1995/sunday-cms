USE [SundayCMS]
GO

IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_modules_getAll')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_modules_getAll] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].sp_modules_getAll
AS
BEGIN
	SELECT * FROM Modules WHERE IsActive = 1
END
GO