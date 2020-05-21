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

IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_users_search')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_users_search] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_users_search]
AS
BEGIN
	SELECT COUNT(*) FROM [Users];
	SELECT * FROM [Users];
END
GO

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
	@RoleIds nvarchar(MAX)
)
AS
BEGIN
	UPDATE [Users]
	SET FullName = @Fullname, Email = @Email, Phone = @Phone, IsActive = @IsActive, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate
	WHERE ID = @ID
	SELECT @ID

	DECLARE @tblRoleIds TABLE (RoleId varchar(100))
	INSERT INTO @tblRoleIds SELECT value  FROM STRING_SPLIT(@RoleIds, ',')
	DELETE FROM UserRoles WHERE UserId = @ID
	INSERT INTO UserRoles (UserId, RoleId) SELECT @ID, RoleId FROM @tblRoleIds
END