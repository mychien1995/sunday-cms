IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Users'))
BEGIN
    CREATE TABLE Users
	(
		ID integer primary key identity(1,1),
		Domain varchar(100) NOT NULL,
		Username nvarchar(100) NOT NULL,
		SecurityStamp varchar(20),
		PasswordHash nvarchar(MAX) NOT NULL,
		Email nvarchar(500),
		Phone nvarchar(500),
		IsActive bit NOT NULL default(1),
		IsLockedOut bit NOT NULL default(0),
		EmailConfirmed bit NOT NULL default(0),
		CreatedDate datetime NOT NULL default(GETDATE()),
		CreatedBy nvarchar(500),
		UpdatedDate datetime NOT NULL default(GETDATE()),
		UpdatedBy nvarchar(500),
		IsDeleted bit NOT NULL default(0)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Roles'))
BEGIN
	CREATE TABLE Roles
	(
		ID integer primary key,
		Code varchar(20) NOT NULL UNIQUE,
		RoleName nvarchar(500),
		[Description] nvarchar(MAX)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'UserRoles'))
BEGIN
	CREATE TABLE UserRoles
	(
		ID integer primary key identity(1,1),
		RoleId integer NOT NULL,
		UserId integer NOT NULL,
		CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId) REFERENCES Roles(ID),
		CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId) REFERENCES Users(ID),
	);
END

IF TYPE_ID(N'RoleType') IS NULL
BEGIN
	CREATE TYPE RoleType AS TABLE
	(
		ID integer,
		Code varchar(20),
		RoleName nvarchar(500),
		[Description] nvarchar(MAX)
	);
END

IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_database_seeding')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_database_seeding] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].[sp_database_seeding]
(
	@PasswordHash nvarchar(MAX),
	@SecurityStamp nvarchar(20),
	@RoleType RoleType READONLY
)
AS
BEGIN
	DECLARE @UserExist bit = 1
	DECLARE @UserId int
	IF NOT EXISTS (select 1 from dbo.Users WHERE Username = 'admin' and Domain = 'CMS')
	BEGIN
		SET @UserExist = 0
		INSERT INTO Users(Username, Domain, EmailConfirmed, CreatedBy, UpdatedBy, SecurityStamp, PasswordHash)
		VALUES ('admin', 'CMS', 1, 'System' , 'System', @SecurityStamp, @PasswordHash)
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