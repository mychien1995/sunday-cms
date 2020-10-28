﻿IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Users'))
BEGIN
    CREATE TABLE Users
	(
		Id uniqueidentifier primary key default(NEWID()),
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
		Id integer primary key,
		Code varchar(20) NOT NULL UNIQUE,
		RoleName nvarchar(500),
		[Description] nvarchar(MAX)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'UserRoles'))
BEGIN
	CREATE TABLE UserRoles
	(
		Id uniqueidentifier primary key default(NEWID()),
		RoleId integer NOT NULL,
		UserId uniqueidentifier NOT NULL,
		CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId) REFERENCES Roles(ID),
		CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId) REFERENCES Users(ID),
	);
END

IF TYPE_ID(N'RoleType') IS NULL
BEGIN
	CREATE TYPE RoleType AS TABLE
	(
		Id integer primary key,
		Code varchar(20),
		RoleName nvarchar(500),
		[Description] nvarchar(MAX)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Organizations'))
BEGIN
	CREATE TABLE Organizations
	(
		Id uniqueidentifier primary key default(NEWID()),
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
		Id uniqueidentifier primary key default(NEWID()),
		UserId uniqueidentifier NOT NULL,
		OrganizationId uniqueidentifier NOT NULL,
		IsActive bit NOT NULL default(1),
		CONSTRAINT FK_OrganizationUsers_Users FOREIGN KEY (UserId) REFERENCES Users(ID),
		CONSTRAINT FK_OrganizationUsers_Organization FOREIGN KEY (OrganizationId) REFERENCES Organizations(ID),
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationRoles'))
BEGIN
	CREATE TABLE OrganizationRoles
	(
		Id uniqueidentifier primary key default(NEWID()),
		RoleCode varchar(20),
		RoleName nvarchar(500),
		OrganizationId uniqueidentifier NOT NULL,
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
		Id uniqueidentifier primary key default(NEWID()),
		OrganizationUserId uniqueidentifier NOT NULL,
		OrganizationRoleId uniqueidentifier NOT NULL,
		CONSTRAINT FK_OrganizationUserRoles_OrganizationUser FOREIGN KEY (OrganizationUserId) REFERENCES OrganizationUsers(ID),
		CONSTRAINT FK_OrganizationUserRoles_OrganizationRole FOREIGN KEY (OrganizationRoleId) REFERENCES OrganizationRoles(ID)
	);
END


IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Modules'))
BEGIN
	CREATE TABLE Modules
	(
		Id uniqueidentifier primary key default(NEWID()),
		ModuleName nvarchar(500),
		ModuleCode varchar(100) UNIQUE,
		IsActive bit
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationModules'))
BEGIN
	CREATE TABLE OrganizationModules
	(
		Id uniqueidentifier primary key default(NEWID()),
		ModuleId uniqueidentifier,
		OrganizationId uniqueidentifier,
		CONSTRAINT FK_OrganizationModule_Module FOREIGN KEY (ModuleId) REFERENCES Modules(ID),
		CONSTRAINT FK_OrganizationModule_Organization FOREIGN KEY (OrganizationId) REFERENCES Organizations(ID)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Features'))
BEGIN
	CREATE TABLE Features
	(
		Id uniqueidentifier primary key default(NEWID()),
		FeatureCode varchar(20) UNIQUE,
		FeatureName nvarchar(500),
		[Description] nvarchar(MAX),
		ModuleId uniqueidentifier NULL,
		CONSTRAINT FK_Feature_Module FOREIGN KEY (ModuleId) REFERENCES Modules(ID)
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationRolesMapping'))
BEGIN
	CREATE TABLE OrganizationRolesMapping
	(
		Id uniqueidentifier primary key default(NEWID()),
		OrganizationRoleId uniqueidentifier NOT NULL,
		FeatureId uniqueidentifier NOT NULL,
		CONSTRAINT FK_OrganizationRolesMapping_OrganizationRole FOREIGN KEY (OrganizationRoleId) REFERENCES OrganizationRoles(ID),
		CONSTRAINT FK_OrganizationRolesMapping_Feature FOREIGN KEY (FeatureId) REFERENCES Features(ID),
	);
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Layouts'))
BEGIN
	CREATE TABLE Layouts
	(
		Id uniqueidentifier primary key default newid(),
		LayoutName nvarchar(1000) not null,
		LayoutPath nvarchar(2000) not null,
		CreatedDate datetime not null,
		CreatedBy varchar(500) not null,
		UpdatedDate datetime not null,
		UpdatedBy varchar(500) not null,
		IsDeleted bit not null default(0)
	)
END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'OrganizationLayouts'))
BEGIN
	CREATE TABLE OrganizationLayouts
	(
		Id uniqueidentifier primary key default newid(),
		OrganizationId uniqueidentifier not null,
		LayoutId uniqueidentifier not null,
		CONSTRAINT FK_OrganizationLayouts_Organization FOREIGN KEY (OrganizationId) REFERENCES Organizations(Id),
		CONSTRAINT FK_OrganizationLayouts_Layout FOREIGN KEY (LayoutId) REFERENCES Layouts(Id),
	)
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
		OrganizationRoleId uniqueidentifier,
		Features nvarchar(MAX)
	);
END

IF TYPE_ID(N'OrganizationUserRoleType') IS NULL
CREATE TYPE [dbo].[OrganizationUserRoleType] AS TABLE(
	[OrganizationId] [uniqueidentifier] NULL,
	[OrganizationRolesId] [nvarchar](max) NULL,
	[IsActive] [bit] NULL
)
GO