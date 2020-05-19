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