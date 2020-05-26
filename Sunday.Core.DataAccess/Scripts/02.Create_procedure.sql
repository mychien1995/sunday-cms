IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_database_seeding')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_database_seeding] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_database_seeding]
(
	@PasswordHash nvarchar(MAX),
	@SecurityStamp nvarchar(500),
	@RoleType RoleType READONLY
)
AS
BEGIN
	DECLARE @UserExist bit = 1
	DECLARE @UserId int
	IF NOT EXISTS (select 1 from dbo.Users WHERE Username = 'admin' and Domain = 'CMS')
	BEGIN
		SET @UserExist = 0
		INSERT INTO Users(Username, FullName, Domain, EmailConfirmed, CreatedBy, UpdatedBy, SecurityStamp, PasswordHash)
		VALUES ('admin', N'System Admin', 'CMS', 1, 'System' , 'System', @SecurityStamp, @PasswordHash)
		SET @UserId = SCOPE_IDENTITY() 
	END

	IF NOT EXISTS (select TOP 1 * from dbo.Roles)
	BEGIN
		INSERT INTO dbo.Roles
		SELECT * FROM @RoleType
	END

	IF (@UserExist = 0)
	BEGIN
		INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, 1)
	END
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_findbyusername')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_findbyusername] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_users_findbyusername]
(
	@Username nvarchar(MAX)
)
AS
BEGIN
	SELECT * FROM [Users] WHERE Username = @Username
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_getById')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_getById] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_users_getById]
(
	@UserId int
)
AS
BEGIN
	SELECT * FROM [Users] WHERE ID = @UserId AND IsDeleted = 0;
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_search')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_search] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_users_search]
(
	@PageIndex int = 0,
	@PageSize int = 10,
	@ExcludeIds nvarchar(MAX) = '',
	@IncludeIds nvarchar(MAX) = '',
	@Text nvarchar(MAX) = '',
	@RoleIds nvarchar(MAX) = '',
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

	DECLARE @WhereClause nvarchar(MAX);
	SET @WhereClause = ' IsDeleted = 0 ';

	IF(@ExcludeIds IS NOT NULL AND LEN(TRIM(@ExcludeIds)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' ID NOT IN (' + @ExcludeIds + ') ';
	END

	IF(@IncludeIds IS NOT NULL AND LEN(TRIM(@IncludeIds)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' ID IN (' + @IncludeIds + ') ';
	END

	IF(@Text IS NOT NULL AND LEN(TRIM(@Text)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' (Username LIKE ''%'' + @Text + ''%'' OR FullName LIKE ''%'' + @Text + ''%'' OR Email LIKE ''%'' + @Text + ''%'') ' ;
	END

	IF(@RoleIds IS NOT NULL AND LEN(TRIM(@RoleIds)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' ID IN (SELECT UserId FROM UserRoles WHERE RoleId IN ('+@RoleIds+')) ';
	END

	IF(LEN(TRIM(@WhereClause)) > 0)
		SET @WhereClause = ' WHERE ' + @WhereClause
	DECLARE @CountQuery nvarchar(MAX);
	SET @CountQuery = 'SELECT COUNT(*) FROM [Users] ' + @WhereClause
	
	DECLARE @DataQuery nvarchar(MAX);
	SET @DataQuery = 'SELECT * FROM [Users] ' + @WhereClause  + ' ORDER BY ' + @SortBy + ' ' + @SortDirection
	+ ' OFFSET ' + CAST(@PageIndex AS VARCHAR(100)) + ' ROWS FETCH NEXT '+ CAST(@PageSize AS VARCHAR(100)) +' ROWS ONLY'

	PRINT @DataQuery

	exec sp_executesql @CountQuery, 
	N'@Text nvarchar(MAX)',
	@Text

	exec sp_executesql @DataQuery, 
	N'@Text nvarchar(MAX)',
	@Text
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_insert')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_insert] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_users_insert]
(
	@UserName nvarchar(500),
	@Fullname nvarchar(500),
	@Email nvarchar(500),
	@Phone nvarchar(500),
	@IsActive bit = 1,
	@Domain nvarchar(500),
	@EmailConfirmed bit = 0,
	@CreatedBy nvarchar(500),
	@UpdatedBy nvarchar(500),
	@SecurityStamp nvarchar(500),
	@PasswordHash nvarchar(1000),
	@RoleIds nvarchar(MAX)
)
AS
BEGIN
	DECLARE @UserId int
	INSERT INTO Users(Username, FullName, Email, Phone, Domain, IsActive, EmailConfirmed, CreatedBy, UpdatedBy, SecurityStamp, PasswordHash)
	VALUES (@UserName, @Fullname, @Email, @Phone , @Domain, @IsActive, @EmailConfirmed, @CreatedBy, @UpdatedBy, @SecurityStamp, @PasswordHash)
	SET @UserId = SCOPE_IDENTITY() 
	SELECT @UserId

	DECLARE @tblRoleIds TABLE (RoleId varchar(100))
	INSERT INTO @tblRoleIds SELECT value  FROM STRING_SPLIT(@RoleIds, ',')
	INSERT INTO UserRoles (UserId, RoleId) SELECT @UserId, RoleId FROM @tblRoleIds
END
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_roles_getAll')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_roles_getAll] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_roles_getAll]
AS
BEGIN
	SELECT * FROM [Roles];
END
GO

IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_getById_withOptions')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_getById_withOptions] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_users_getById_withOptions]
(
	@UserId integer,
	@FetchRoles bit = 0,
	@FetchOrganizations bit = 0
)
AS
BEGIN
	SELECT * FROM Users WHERE ID = @UserId AND IsDeleted = 0;
	IF @FetchRoles = 1
	BEGIN
		SELECT * FROM Roles WHERE ID IN (SELECT RoleId FROM UserRoles WHERE UserId = @UserId)
	END
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_update')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_update] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_users_update]
(
	@ID int,
	@Fullname nvarchar(500),
	@Email nvarchar(500),
	@Phone nvarchar(500),
	@IsActive bit = 1,
	@UpdatedBy nvarchar(500),
	@UpdatedDate datetime,
	@AvatarBlobUri nvarchar(MAX),
	@RoleIds nvarchar(MAX)
)
AS
BEGIN
	UPDATE [Users] SET FullName = @Fullname, Email = @Email, Phone = @Phone, IsActive = @IsActive, UpdatedBy = @UpdatedBy,
	UpdatedDate = @UpdatedDate, AvatarBlobUri = @AvatarBlobUri
	WHERE ID = @ID
	SELECT @ID
	
	IF(@RoleIds IS NOT NULL AND LEN(TRIM(@RoleIds)) > 0)
	BEGIN
		DECLARE @tblRoleIds TABLE (RoleId varchar(100))
		INSERT INTO @tblRoleIds SELECT value  FROM STRING_SPLIT(@RoleIds, ',')
		DELETE FROM UserRoles WHERE UserId = @ID
		INSERT INTO UserRoles (UserId, RoleId) SELECT @ID, RoleId FROM @tblRoleIds
	END
END
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_delete')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_delete] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_users_delete]
(
	@UserId integer
)
AS
BEGIN
	UPDATE [Users] Set IsDeleted = 1 WHERE ID = @UserId
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_fetchRoles')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_fetchRoles] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE dbo.sp_users_fetchRoles
(
	@UserIds nvarchar(MAX)
)
AS
BEGIN
	IF(@UserIds IS NULL OR LEN(TRIM(@UserIds)) = 0)
	BEGIN
		SELECT * FROM Roles WHERE 1 = 2
	END
	DECLARE @tblUserIds TABLE (ID varchar(100))
	INSERT INTO @tblUserIds SELECT value  FROM STRING_SPLIT(@UserIds, ',')

	SELECT UserId, RoleId, Code, RoleName 
	FROM UserRoles, Roles 
	WHERE UserId IN (SELECT ID FROM @tblUserIds)
	AND UserRoles.RoleId = Roles.ID

END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_activate')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_activate] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE dbo.sp_users_activate
(
	@UserId int
)
AS
BEGIN
	UPDATE Users SET IsActive = 1 WHERE ID = @UserId
END
GO

IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_deactivate')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_deactivate] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE dbo.sp_users_deactivate
(
	@UserId int
)
AS
BEGIN
	UPDATE Users SET IsActive = 0 WHERE ID = @UserId
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_changePassword')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_changePassword] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE dbo.sp_users_changePassword
(
	@UserId int,
	@SecurityHash nvarchar(MAX),
	@PasswordHash nvarchar(MAX)
)
AS
BEGIN
	UPDATE Users SET SecurityStamp = @SecurityHash, PasswordHash = @PasswordHash WHERE ID = @UserId
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_roles_getById')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_roles_getById] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE dbo.sp_roles_getById
(
	@RoleId int
)
AS
BEGIN
	SELECT * FROM Roles WHERE ID = @RoleId
END
GO