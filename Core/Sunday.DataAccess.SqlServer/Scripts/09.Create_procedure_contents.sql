﻿CREATE OR ALTER PROCEDURE sp_content_create
(
	@Id uniqueidentifier,
	@Name nvarchar(1000),
	@DisplayName nvarchar(1000),
	@Path nvarchar(MAX),
	@ParentId uniqueidentifier,
	@ParentType int,
	@TemplateId uniqueidentifier,
	@CreatedDate datetime,
	@UpdatedDate datetime,
	@CreatedBy varchar(500),
	@UpdatedBy varchar(500),
	@SortOrder int,
	@WorkId uniqueidentifier,
	@Fields ContentFieldType READONLY
)
AS
BEGIN
	INSERT INTO [dbo].[Contents]
	([Id], [Name], [DisplayName], [Path], [ParentId], [ParentType], [TemplateId], [CreatedDate], 
	[UpdatedDate], [CreatedBy], [UpdatedBy], [SortOrder])
	VALUES (@Id, @Name, @DisplayName, @Path, @ParentId, @ParentType, @TemplateId, @CreatedDate,
	@UpdatedDate, @CreatedBy, @UpdatedBy, @SortOrder);

	INSERT INTO [dbo].[WorkContents]
	([Id], [ContentId], [CreatedDate], 
	  [UpdatedDate], [CreatedBy], [UpdatedBy], [Version], [Status], [IsActive], [IsDeleted])
     VALUES
    (@WorkId, @Id, @CreatedDate, 
	  @UpdatedDate, @CreatedBy, @UpdatedBy, 1, 1, 1, 0);

	INSERT INTO WorkContentFields (Id, FieldValue, TemplateFieldId, WorkContentId)
	SELECT NEWID(), FieldValue, TemplateFieldId, @WorkId FROM @Fields
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_content_update]
(
	@Id uniqueidentifier,
	@Name nvarchar(1000),
	@DisplayName nvarchar(1000),
	@Path nvarchar(MAX),
	@ParentId uniqueidentifier,
	@ParentType int,
	@UpdatedDate datetime,
	@UpdatedBy varchar(500),
	@SortOrder int,
	@WorkId uniqueidentifier,
	@Fields ContentFieldType READONLY
)
AS
BEGIN
  UPDATE [dbo].[Contents]
  SET [Name] = @Name,
      [DisplayName] = @DisplayName,
      [Path] = @Path,
      [ParentId] = @ParentId,
      [ParentType] = @ParentType,
      [UpdatedDate] = @UpdatedDate,
      [UpdatedBy] = @UpdatedBy,
      [SortOrder] = @SortOrder
  WHERE Id = @Id
  
  DECLARE @IsWorkVersionActive bit 
  SET @IsWorkVersionActive = (SELECT IsActive FROM WorkContents WHERE Id = @WorkId AND ContentId = @Id)
  IF (@IsWorkVersionActive = 1)
  BEGIN
      UPDATE [dbo].[WorkContents]
	  SET [UpdatedDate] = @UpdatedDate,
		  [UpdatedBy] = @UpdatedBy
	  WHERE Id = @WorkId
	
	  DELETE FROM WorkContentFields WHERE WorkContentId = @WorkId AND Id NOT IN (SELECT Id FROM @Fields)
	  MERGE WorkContentFields AS target
	  USING (SELECT * FROM @Fields) AS source
	  ON target.id = source.Id
	  WHEN MATCHED THEN 
		UPDATE SET target.FieldValue = source.FieldValue, target.TemplateFieldId = source.TemplateFieldId
	  WHEN NOT MATCHED THEN 
		INSERT (Id, FieldValue, TemplateFieldId, WorkContentId)
		VALUES (NEWID(), source.FieldValue, source.TemplateFieldId, @WorkId);
  END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_content_delete]
(
	@Id uniqueidentifier
)
AS
BEGIN
	UPDATE Contents SET IsDeleted = 1 WHERE Id = @Id
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_content_newVersion]
(
	@Id uniqueidentifier,
	@UpdatedDate datetime,
	@UpdatedBy varchar(500),
	@FromVersion uniqueidentifier
)
AS
BEGIN
	DECLARE @LastVersion int
	SET @LastVersion = (SELECT MAX(Version) FROM WorkContents WHERE ContentId = @Id)
	DECLARE @WorkId uniqueidentifier 
	SET @WorkId = NEWID()

	INSERT INTO WorkContents
	(Id, ContentId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, Version, Status, IsActive)
	VALUES (@WorkId, @Id, @UpdatedDate, @UpdatedBy, @UpdatedDate, @Updatedby, @LastVersion + 1, 1, 1)

	UPDATE WorkContents SET IsActive = 0 WHERE ContentId = @Id AND Id <> @WorkId

	INSERT INTO WorkContentFields
	(Id, WorkContentId, FieldValue, TemplateFieldId)
	SELECT NEWID(), @WorkId, FieldValue, TemplateFieldId FROM WorkContentFields WHERE WorkContentId = @FromVersion
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_content_publish]
(
	@Id uniqueidentifier,
	@PublishedBy varchar(200),
	@PublishedDate datetime
)
AS
BEGIN
	DECLARE @LatestVersionId uniqueidentifier
	SET @LatestVersionId = (SELECT TOP 1 Id FROM WorkContents WHERE ContentId = @Id AND Status <> 2 AND IsActive = 1)
	IF @LatestVersionId IS NOT NULL
	BEGIN
		UPDATE Contents SET PublishedBy = @PublishedBy, PublishedDate  = @PublishedDate
		WHERE Id = @Id

		DELETE FROM ContentFields WHERE ContentId = @Id
		INSERT INTO ContentFields (Id, ContentId, FieldValue, TemplateFieldId)
		SELECT NEWID(), @Id, FieldValue, TemplateFieldId FROM WorkContentFields WHERE WorkContentId = @LatestVersionId
	END
END
GO
