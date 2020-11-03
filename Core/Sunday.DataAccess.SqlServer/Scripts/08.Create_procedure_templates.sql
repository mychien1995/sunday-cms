CREATE OR ALTER PROCEDURE sp_templates_save
(
	 @Id uniqueidentifier
	,@TemplateName nvarchar(500)
	,@Icon varchar(100)
	,@StandardValueId uniqueidentifier = NULL
	,@BaseTemplateIds nvarchar(max) = ''
	,@HasRestrictions bit = 0
	,@CreatedDate datetime
	,@CreatedBy nvarchar(500)
	,@UpdatedDate datetime
	,@UpdatedBy nvarchar(500)
	,@IsUpdate bit
)
AS
BEGIN
	IF @IsUpdate = 0
	BEGIN
	INSERT INTO [dbo].[Templates]
           ([Id] ,[TemplateName] ,[Icon] ,[StandardValueId] ,[BaseTemplateIds]
           ,[HasRestrictions] ,[CreatedDate] ,[CreatedBy],[UpdatedDate] ,[UpdatedBy] ,[IsDeleted])
     VALUES
           (@Id ,@TemplateName ,@Icon ,@StandardValueId ,@BaseTemplateIds
			,@HasRestrictions ,@CreatedDate ,@CreatedBy ,@UpdatedDate ,@UpdatedBy ,0)
	END
	ELSE
	BEGIN
		UPDATE dbo.Templates SET TemplateName = @TemplateName, Icon = @Icon, StandardValueId = @StandardValueId,
		BaseTemplateIds = @BaseTemplateIds, HasRestrictions = @HasRestrictions, UpdatedDate = @UpdatedDate,
		UpdatedBy = @UpdatedBy
		WHERE Id = @Id
	END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_templates_delete
(
	 @Id uniqueidentifier
)
AS
BEGIN
	UPDATE Templates SET IsDeleted = 0 WHERE Id = @Id
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_templates_search
(
	@PageIndex int = 0,
	@PageSize int = 10,
	@Text nvarchar(1000)
)
AS
BEGIN
	SELECT COUNT(*) FROM dbo.Templates WHERE IsDeleted = 0
	AND 
	@Text IS NULL OR LEN(TRIM(@Text)) = 0
	OR CAST(Id as varchar) = @Text
	OR TemplateName LIKE '%' + @Text + '%'

	SELECT * FROM dbo.Templates WHERE IsDeleted = 0
	AND 
	@Text IS NULL OR LEN(TRIM(@Text)) = 0
	OR CAST(Id as varchar) = @Text
	OR TemplateName LIKE '%' + @Text + '%'
	ORDER BY UpdatedDate DESC
	OFFSET @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_templates_getById
(
	@Id uniqueidentifier
)
AS
BEGIN
	SELECT * FROM dbo.Templates WHERE IsDeleted = 0 AND Id = @Id
	SELECT * FROM dbo.TemplateFields WHERE TemplateId = @Id
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_templates_saveProperties
(
	@TemplateId uniqueidentifier,
	@Fields TemplateFieldType READONLY
)
AS
BEGIN
	DECLARE @TemplateExist int 
	SET @TemplateExist = (SELECT COUNT (*) FROM Templates WHERE Id = @TemplateId)
	IF @TemplateExist = 1
	BEGIN
		MERGE TemplateFields AS target  
			USING (SELECT *, @TemplateId AS TemplateId FROM @Fields) AS source
		ON (target.Id = source.Id)  
		WHEN MATCHED THEN
			UPDATE SET target.FieldName = source.FieldName, target.DisplayName = source.DisplayName,
					   target.FieldType = source.FieldType, target.Title = source.Title,
					   target.IsUnversioned = source.IsUnversioned, target.Properties = source.Properties,
					   target.Section = source.Section, target.ValidationRules = source.ValidationRules,
					   target.SortOrder = source.SortOrder
		WHEN NOT MATCHED THEN  
			INSERT (Id, FieldName, DisplayName, FieldType, Title, IsUnversioned, Properties, Section, ValidationRules, TemplateId, IsDeleted, SortOrder)  
			VALUES (Id, source.FieldName, source.DisplayName, source.FieldType, source.Title, source.IsUnversioned, source.Properties, 
			source.Section, source.ValidationRules, @TemplateId, 0, source.SortOrder);  
		
		UPDATE TemplateFields SET IsDeleted = 1 WHERE TemplateId = @TemplateId AND Id NOT IN (SELECT Id FROM @Fields)
	END

END
GO

