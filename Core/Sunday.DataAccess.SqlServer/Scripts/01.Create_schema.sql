IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Users'))
BEGIN
    CREATE TABLE Users
	(
		ID integer primary key identity(1,1),
		Domain varchar(100) NOT NULL,
		Username nvarchar(100) NOT NULL,
		FullName nvarchar(500),
		SecurityStamp varchar(500),
		PasswordHash nvarchar(MAX) NOT NULL,
		Email nvarchar(500),
		Phone nvarchar(500),
		AvatarBlobUri nvarchar(MAX),
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

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Organizations'))
BEGIN
	CREATE TABLE Organizations
	(
		ID integer primary key identity(1,1),
		OrganizationName nvarchar(500),
		[Description] nvarchar(MAX),
		ExtraProperties nvarchar(MAX),
		Hosts nvarchar(MAX),
		LogoBlobUri nvarchar(MAX),
		IsActive bit NOT NULL default(0),
		CreatedDate datetime NOT NULL default(GETDATE()),
		CreatedBy nvarchar(100),
		UpdatedDate datetime NOT NULL default(GETDATE()),
		UpdatedBy nvarchar(100),
		IsDeleted bit NOT NULL default(0)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationUsers'))
BEGIN
	CREATE TABLE OrganizationUsers
	(
		ID integer primary key identity(1,1),
		UserId integer NOT NULL,
		OrganizationId integer NOT NULL,
		IsActive bit NOT NULL default(1),
		CONSTRAINT FK_OrganizationUsers_Users FOREIGN KEY (UserId) REFERENCES Users(ID),
		CONSTRAINT FK_OrganizationUsers_Organization FOREIGN KEY (OrganizationId) REFERENCES Organizations(ID),
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationRoles'))
BEGIN
	CREATE TABLE OrganizationRoles
	(
		ID integer primary key identity(1,1),
		RoleCode varchar(20),
		RoleName nvarchar(500),
		OrganizationId integer NOT NULL,
		[Description] nvarchar(MAX),
		CreatedDate datetime NOT NULL default(GETDATE()),
		CreatedBy nvarchar(100),
		UpdatedDate datetime NOT NULL default(GETDATE()),
		UpdatedBy nvarchar(100),
		IsDeleted bit NOT NULL default(0),
		CONSTRAINT FK_OrganizationRoles_Organization FOREIGN KEY (OrganizationId) REFERENCES Organizations(ID),
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationUserRoles'))
BEGIN
	CREATE TABLE OrganizationUserRoles
	(
		ID integer primary key identity(1,1),
		OrganizationUserId integer NOT NULL,
		OrganizationRoleId integer NOT NULL,
		CONSTRAINT FK_OrganizationUserRoles_OrganizationUser FOREIGN KEY (OrganizationUserId) REFERENCES OrganizationUsers(ID),
		CONSTRAINT FK_OrganizationUserRoles_OrganizationRole FOREIGN KEY (OrganizationRoleId) REFERENCES OrganizationRoles(ID)
	);
END


IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Modules'))
BEGIN
	CREATE TABLE Modules
	(
		ID int primary key identity(1,1),
		ModuleName nvarchar(500),
		ModuleCode varchar(100) UNIQUE,
		IsActive bit
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationModules'))
BEGIN
	CREATE TABLE OrganizationModules
	(
		ID integer primary key identity(1,1),
		ModuleId int,
		OrganizationId int,
		CONSTRAINT FK_OrganizationModule_Module FOREIGN KEY (ModuleId) REFERENCES Modules(ID),
		CONSTRAINT FK_OrganizationModule_Organization FOREIGN KEY (OrganizationId) REFERENCES Organizations(ID)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Features'))
BEGIN
	CREATE TABLE Features
	(
		ID integer primary key identity(1,1),
		FeatureCode varchar(20) UNIQUE,
		FeatureName nvarchar(500),
		[Description] nvarchar(MAX),
		ModuleId int NULL,
		CONSTRAINT FK_Feature_Module FOREIGN KEY (ModuleId) REFERENCES Modules(ID)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationRolesMapping'))
BEGIN
	CREATE TABLE OrganizationRolesMapping
	(
		ID integer primary key identity(1,1),
		OrganizationRoleId integer NOT NULL,
		FeatureId integer NOT NULL,
		CONSTRAINT FK_OrganizationRolesMapping_OrganizationRole FOREIGN KEY (OrganizationRoleId) REFERENCES OrganizationRoles(ID),
		CONSTRAINT FK_OrganizationRolesMapping_Feature FOREIGN KEY (FeatureId) REFERENCES Features(ID),
	);
END

IF TYPE_ID(N'OrganizationUserType') IS NULL
BEGIN
	CREATE TYPE OrganizationUserType AS TABLE
	(
		OrganizationId int,
		IsActive bit
	);
END

IF TYPE_ID(N'ModuleType') IS NULL
BEGIN
	CREATE TYPE ModuleType AS TABLE
	(
		Code nvarchar(MAX),
		[Name] nvarchar(MAX)
	);
END

IF TYPE_ID(N'FeatureType') IS NULL
BEGIN
	CREATE TYPE FeatureType AS TABLE
	(
		Code nvarchar(MAX),
		[Name] nvarchar(MAX),
		[ModuleCode] nvarchar(MAX)
	);
END

IF TYPE_ID(N'OrganizationRoleType') IS NULL
BEGIN
	CREATE TYPE OrganizationRoleType AS TABLE
	(
		OrganizationRoleId int,
		Features nvarchar(MAX)
	);
END