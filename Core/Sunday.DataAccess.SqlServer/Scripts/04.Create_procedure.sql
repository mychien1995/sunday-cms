IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_modules_getAll')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_modules_getAll] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].sp_modules_getAll
AS
BEGIN
	SELECT * FROM Modules WHERE IsActive = 1
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_features_getByModules')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_features_getByModules] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].sp_features_getByModules
(
	@ModulesIds nvarchar(MAX)
)
AS
BEGIN
	IF(@ModulesIds IS NOT NULL AND LEN(TRIM(@ModulesIds)) > 0)
	BEGIN
		DECLARE @tblModuleIds TABLE (ModuleId varchar(100))
		INSERT INTO @tblModuleIds SELECT value  FROM STRING_SPLIT(@ModulesIds, ',')
		SELECT * FROM Features WHERE ModuleId IN (SELECT ModuleId FROM @tblModuleIds)
		ORDER BY ModuleId ASC, ID ASC
	END
	ELSE
	BEGIN
		SELECT * FROM Features WHERE 1 = 2
	END
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizationRoles_getByOrganization')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizationRoles_getByOrganization] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].sp_organizationRoles_getByOrganization
(
	@OrganizationId int,
	@PageIndex int = 0,
	@PageSize int = 10
)
AS
BEGIN
	If @PageIndex IS NULL
		SET @PageIndex = 0
	If @PageSize IS NULL
		SET @PageSize = 10

	SELECT COUNT(*) FROM OrganizationRoles  WHERE OrganizationId = @OrganizationId AND IsDeleted = 0
	SELECT * FROM OrganizationRoles WHERE OrganizationId = @OrganizationId AND IsDeleted = 0
	ORDER BY UpdatedDate DESC
	OFFSET @PageIndex ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizationRoles_getById')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizationRoles_getById] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].sp_organizationRoles_getById
(
	@OrganizationRoleId int
)
AS
BEGIN
	SELECT * FROM OrganizationRoles WHERE ID = @OrganizationRoleId AND IsDeleted = 0
	SELECT * FROM Features WHERE ID IN (SELECT FeatureId FROM OrganizationRolesMapping WHERE OrganizationRoleId = @OrganizationRoleId)
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizationRoles_create')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizationRoles_create] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].sp_organizationRoles_create
(
	@RoleCode nvarchar(100) = NULL,
	@RoleName nvarchar(MAX),
	@OrganizationId int,
	@Description nvarchar(MAX),
	@CreatedDate datetime,
	@CreatedBy nvarchar(MAX),
	@UpdatedDate datetime,
	@UpdatedBy nvarchar(MAX),
	@FeatureIds nvarchar(MAX)
)
AS
BEGIN
	DECLARE @OrganizationRoleId int

	INSERT INTO OrganizationRoles
	(RoleCode, RoleName, OrganizationId, [Description], CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
	VALUES
	(@RoleCode, @RoleName, @OrganizationId, @Description, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)

	SET @OrganizationRoleId = SCOPE_IDENTITY()
	SELECT @OrganizationRoleId

	IF(@FeatureIds IS NOT NULL AND LEN(TRIM(@FeatureIds)) > 0)
	BEGIN
		DECLARE @tblFeatureId TABLE (FeatureId Int)
		INSERT INTO @tblFeatureId  SELECT value  FROM STRING_SPLIT(@FeatureIds, ',')
		INSERT INTO OrganizationRolesMapping (OrganizationRoleId, FeatureId) SELECT @OrganizationRoleId, FeatureId FROM @tblFeatureId
	END
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizationRoles_update')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizationRoles_update] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].sp_organizationRoles_update
(
	@RoleId int,
	@RoleName nvarchar(MAX),
	@Description nvarchar(MAX),
	@UpdatedDate datetime,
	@UpdatedBy nvarchar(MAX),
	@FeatureIds nvarchar(MAX)
)
AS
BEGIN

	UPDATE OrganizationRoles
	SET RoleName = @RoleName, [Description] = @Description, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate
	WHERE ID = @RoleId

	IF(@FeatureIds IS NOT NULL AND LEN(TRIM(@FeatureIds)) > 0)
	BEGIN
		DECLARE @tblFeatureId TABLE (FeatureId Int)
		INSERT INTO @tblFeatureId  SELECT value  FROM STRING_SPLIT(@FeatureIds, ',')

		DELETE FROM OrganizationRolesMapping WHERE OrganizationRoleId = @RoleId AND FeatureId NOT IN (SELECT FeatureId FROM @tblFeatureId)

		INSERT INTO OrganizationRolesMapping (OrganizationRoleId, FeatureId) SELECT @RoleId, FeatureId FROM @tblFeatureId
		WHERE FeatureId NOT IN (SELECT FeatureId FROM OrganizationRolesMapping WHERE OrganizationRoleId = @RoleId)
	END
	ELSE
	BEGIN
		DELETE FROM OrganizationRolesMapping WHERE OrganizationRoleId = @RoleId
	END

	SELECT * FROM OrganizationRoles WHERE ID = @RoleId
END
GO
--------------------------------------------------------------------
IF NOT EXISTS (select 1 from sys.procedures where name = 'sp_organizationRoles_delete')
BEGIN
	EXEC('CREATE PROCEDURE [dbo].[sp_organizationRoles_delete] AS BEGIN SET NOCOUNT ON; END')
END
GO
ALTER PROCEDURE [dbo].sp_organizationRoles_delete
(
	@RoleId int
)
AS
BEGIN
	UPDATE OrganizationRoles SET IsDeleted = 1 WHERE ID = @RoleId
END
GO