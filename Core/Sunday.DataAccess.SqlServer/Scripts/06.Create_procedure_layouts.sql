CREATE OR ALTER PROCEDURE sp_layouts_search
(
	@PageIndex int = 0,
	@PageSize int = 10,
	@LayoutId uniqueidentifier = NULL,
	@OrganizationId uniqueidentifier = NULL
)
AS
BEGIN
	IF @PageIndex IS NULL SET @PageIndex = 0;
	IF @PageSize IS NULL SET @PageSize = 10;
	IF @LayoutId IS NULL
	BEGIN
		SELECT COUNT(*) FROM Layouts WHERE IsDeleted = 0 AND 
		(@OrganizationId IS NULL OR Id IN (SELECT LayoutId FROM OrganizationLayouts WHERE OrganizationId = @OrganizationId))

		SELECT * FROM Layouts WHERE IsDeleted = 0 AND 
			(@OrganizationId IS NULL OR Id IN (SELECT LayoutId FROM OrganizationLayouts WHERE OrganizationId = @OrganizationId))
		ORDER BY UpdatedDate DESC OFFSET @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY

		SELECT TOP 0 * FROM OrganizationLayouts
	END
	ELSE
	BEGIN
		SELECT COUNT(*) FROM Layouts WHERE IsDeleted = 0 AND Id = @LayoutId
		SELECT * FROM Layouts WHERE IsDeleted = 0 AND Id = @LayoutId
		SELECT * FROM OrganizationLayouts WHERE LayoutId = @LayoutId
	END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_layouts_insert
(
	@Id uniqueidentifier,
	@LayoutName nvarchar(MAX),
	@LayoutPath nvarchar(MAX),
	@CreatedDate datetime,
	@CreatedBy nvarchar(MAX),
	@UpdatedDate datetime,
	@UpdatedBy nvarchar(MAX),
	@Organizations nvarchar(MAX)
)
AS
BEGIN
	INSERT INTO [dbo].[Layouts]
           ([Id]
           ,[LayoutName]
           ,[LayoutPath]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[IsDeleted])
     VALUES
           (@Id,
			@LayoutName,
			@LayoutPath,
			@CreatedDate,
			@CreatedBy,
			@UpdatedDate,
			@UpdatedBy,
			0)

	IF(@Organizations IS NOT NULL AND LEN(TRIM(@Organizations)) > 0)
	BEGIN
		DECLARE @tblOrganizationId TABLE (OrganizationId uniqueidentifier)
		INSERT INTO @tblOrganizationId  SELECT value  FROM STRING_SPLIT(@Organizations, '|')
		INSERT INTO OrganizationLayouts (OrganizationId, LayoutId) SELECT OrganizationId, @Id FROM @tblOrganizationId
	END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_layouts_update
(
	@Id uniqueidentifier,
	@LayoutName nvarchar(MAX),
	@LayoutPath nvarchar(MAX),
	@UpdatedDate datetime,
	@UpdatedBy nvarchar(MAX),
	@IsDeleted bit = 0,
	@Organizations nvarchar(MAX)
)
AS
BEGIN
	UPDATE Layouts SET LayoutName = @LayoutName, LayoutPath = @LayoutPath, UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy
	WHERE Id = @Id

	BEGIN
		DECLARE @tblOrganizationId TABLE (OrganizationId uniqueidentifier)
		IF(@Organizations IS NOT NULL AND LEN(TRIM(@Organizations)) > 0)
			INSERT INTO @tblOrganizationId  SELECT value  FROM STRING_SPLIT(@Organizations, '|')
		DELETE FROM OrganizationLayouts WHERE LayoutId = @Id AND OrganizationId NOT IN (SELECT OrganizationId FROM @tblOrganizationId)
		INSERT INTO OrganizationLayouts (OrganizationId, LayoutId) SELECT OrganizationId, @Id FROM @tblOrganizationId
		WHERE OrganizationId NOT IN (SELECT OrganizationId FROM OrganizationLayouts WHERE LayoutId = @Id)
	END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_layouts_delete
(
	@Id uniqueidentifier
)
AS
BEGIN
	DELETE OrganizationLayouts WHERE LayoutId = @Id
	DELETE Layouts WHERE Id = @Id
END
GO