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

IF TYPE_ID(N'TemplateFieldType') IS NULL
BEGIN
	CREATE TYPE TemplateFieldType AS TABLE
	(
		Id uniqueidentifier,
		FieldName varchar(500),
		DisplayName nvarchar(1000),
		FieldType int,
		Title varchar(1000),
		IsUnversioned bit default(0),
		Properties nvarchar(MAX),
		Section varchar(500),
		SortOrder int,
		ValidationRules varchar(500)
	);
END