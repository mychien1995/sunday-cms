IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizations_create')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizations_create] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_organizations_create]
(
	@OrganizationName nvarchar(500),
	@Description nvarchar(MAX),
	@Properties nvarchar(MAX),
	@HostNames nvarchar(MAX),
	@LogoBlobUri nvarchar(MAX),
	@IsActive bit = 0,
	@CreatedDate datetime,
	@CreatedBy nvarchar(100),
	@UpdatedDate datetime,
	@UpdatedBy nvarchar(100),
	@ModuleIds nvarchar(MAX) = ''
)
AS
BEGIN
	IF @IsActive IS NULL
		SET @IsActive = 0
	IF @CreatedDate IS NULL
		SET @CreatedDate = GETDATE()
	IF @UpdatedDate IS NULL
		SET @UpdatedDate  = GETDATE()
	IF @ModuleIds IS NULL
		SET @ModuleIds  = ''

	DECLARE @OrganizationId int
	INSERT INTO [dbo].[Organizations]
           ([OrganizationName]
           ,[Description]
           ,[ExtraProperties]
           ,[Hosts]
           ,[LogoBlobUri]
           ,[IsActive]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[IsDeleted])
     VALUES
           (@OrganizationName
           ,@Description
           ,@Properties
           ,@HostNames
           ,@LogoBlobUri
           ,@IsActive
           ,@CreatedDate
           ,@CreatedBy
           ,@UpdatedDate
           ,@UpdatedBy
           ,0)
	
	SET @OrganizationId = SCOPE_IDENTITY()
	SELECT @OrganizationId

	DECLARE @tblModules TABLE (ModuleId varchar(100))
	INSERT INTO @tblModules SELECT value FROM STRING_SPLIT(@ModuleIds, ',')
	INSERT INTO OrganizationModules (OrganizationId, ModuleId) SELECT @OrganizationId, ModuleId FROM @tblModules

END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizations_update')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizations_update] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_organizations_update]
(
	@ID int,
	@OrganizationName nvarchar(500),
	@Description nvarchar(MAX),
	@Properties nvarchar(MAX),
	@HostNames nvarchar(MAX),
	@LogoBlobUri nvarchar(MAX),
	@IsActive bit = 0,
	@UpdatedDate datetime,
	@UpdatedBy nvarchar(100),
	@ModuleIds nvarchar(MAX) = ''
)
AS
BEGIN
	IF @IsActive IS NULL
		SET @IsActive = 0
	IF @UpdatedDate IS NULL
		SET @UpdatedDate  = GETDATE()
	IF @ModuleIds IS NULL
		SET @ModuleIds  = ''


	UPDATE [dbo].[Organizations]
	SET [OrganizationName] = @OrganizationName
		  ,[Description] = @Description
		  ,[ExtraProperties] = @Properties
		  ,[Hosts] = @HostNames
		  ,[LogoBlobUri] = @LogoBlobUri
		  ,[IsActive] = @IsActive
		  ,[UpdatedDate] = @UpdatedDate
		  ,[UpdatedBy] = @UpdatedBy
	WHERE ID = @ID

	SELECT * FROM Organizations WHERE ID = @ID
	
	DELETE FROM OrganizationModules WHERE OrganizationId = @ID
	BEGIN
		DECLARE @tblModules TABLE (ModuleId varchar(100))
		INSERT INTO @tblModules SELECT value FROM STRING_SPLIT(@ModuleIds, ',')
		INSERT INTO OrganizationModules (OrganizationId, ModuleId) SELECT @ID, ModuleId FROM @tblModules
	END

END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizations_activate')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizations_activate] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_organizations_activate]
(
	@OrganizationId int
)
AS
BEGIN
	UPDATE Organizations SET IsActive = 1 WHERE ID = @OrganizationId
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizations_deactivate')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizations_deactivate] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_organizations_deactivate]
(
	@OrganizationId int
)
AS
BEGIN
	UPDATE Organizations SET IsActive = 0 WHERE ID = @OrganizationId
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizations_delete')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizations_delete] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_organizations_delete]
(
	@OrganizationId int
)
AS
BEGIN
	UPDATE Organizations SET IsDeleted = 1 WHERE ID = @OrganizationId
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizations_getById')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizations_getById] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_organizations_getById]
(
	@OrganizationId int
)
AS
BEGIN
	SELECT * FROM Organizations WHERE ID = @OrganizationId AND IsDeleted = 0
	SELECT * FROM Modules WHERE IsActive = 1 AND ID IN (SELECT ModuleId FROM OrganizationModules WHERE OrganizationId = @OrganizationId)
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizations_search')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizations_search] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_organizations_search]
(
	@PageIndex int = 0,
	@PageSize int = 10,
	@Text nvarchar(MAX) = '',
	@IsActive bit,
	@SortBy nvarchar(MAX) = 'UpdatedDate',
	@SortDirection nvarchar(MAX) = 'DESC'
)
AS
BEGIN
	IF @PageIndex IS NULL
		SET @PageIndex = 0
	IF @PageSize IS NULL
		SET @PageSize = 10
	SET @PageIndex = @PageIndex * @PageSize
	IF @SortBy IS NULL
		SET @SortBy = 'UpdatedDate'
	IF @SortDirection IS NULL
		SET @SortDirection = 'DESC'
	
	DECLARE @WhereClause nvarchar(MAX);
	SET @WhereClause = ' IsDeleted = 0 ';

	IF(@Text IS NOT NULL AND LEN(TRIM(@Text)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' (OrganizationName LIKE ''%'' + @Text + ''%'') ' ;
	END

	IF(@IsActive IS NOT NULL)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' (IsActive = @IsActive) ' ;
	END

	IF(LEN(TRIM(@WhereClause)) > 0)
		SET @WhereClause = ' WHERE ' + @WhereClause
	DECLARE @CountQuery nvarchar(MAX);
	SET @CountQuery = 'SELECT COUNT(*) FROM [Organizations] ' + @WhereClause
	
	DECLARE @DataQuery nvarchar(MAX);
	SET @DataQuery = 'SELECT * FROM [Organizations] ' + @WhereClause  + ' ORDER BY ' + @SortBy + ' ' + @SortDirection
	+ ' OFFSET ' + CAST(@PageIndex AS VARCHAR(100)) + ' ROWS FETCH NEXT '+ CAST(@PageSize AS VARCHAR(100)) +' ROWS ONLY'

	exec sp_executesql @CountQuery, 
	N'@Text nvarchar(MAX), @IsActive bit',
	@Text, @IsActive

	exec sp_executesql @DataQuery, 
	N'@Text nvarchar(MAX), @IsActive bit',
	@Text, @IsActive
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizations_findByHostName')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizations_findByHostName] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE dbo.sp_organizations_findByHostName
(
	@Hostname nvarchar(MAX)
)
AS
BEGIN
	SELECT * FROM [Organizations] WHERE Hosts LIKE '%|' + @Hostname + '%'
	OR Hosts LIKE '%' + @Hostname + '|%' OR Hosts = @Hostname
END
GO