CREATE OR ALTER PROCEDURE dbo.sp_contentLinks_save
@ContentId uniqueidentifier,
@ReferenceIds nvarchar(MAX)
AS
BEGIN
	IF @ReferenceIds IS NOT NULL AND LEN(TRIM(@ReferenceIds)) > 0
	BEGIN
		DELETE FROM ContentLinks WHERE ContentId = @ContentId
		INSERT INTO ContentLinks SELECT @ContentId, value from string_split(@ReferenceIds, '|')
	END
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_contentLinks_getReferenceFrom
@ContentId uniqueidentifier
AS
BEGIN
	SELECT ReferenceId FROM ContentLinks WHERE ContentId = @ContentId
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_contentLinks_getReferenceTo
@ContentId uniqueidentifier
AS
BEGIN
	SELECT ContentId FROM ContentLinks WHERE ReferenceId = @ContentId
END
GO