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
	SELECT * FROM [Users] WHERE ID = @UserId
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
	@PasswordHash nvarchar(1000)
)
AS
BEGIN
	DECLARE @UserId int
	INSERT INTO Users(Username, FullName, Email, Phone, Domain, IsActive, EmailConfirmed, CreatedBy, UpdatedBy, SecurityStamp, PasswordHash)
	VALUES (@UserName, @Fullname, @Email, @Phone , @Domain, @IsActive, @EmailConfirmed, @CreatedBy, @UpdatedBy, @SecurityStamp, @PasswordHash)
	SET @UserId = SCOPE_IDENTITY() 
	SELECT @UserId
END
GO