CREATE OR ALTER PROCEDURE [dbo].sp_modules_getAll
AS
BEGIN
	SELECT * FROM Modules WHERE IsActive = 1
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].sp_features_getByModules
(
	@ModulesIds nvarchar(MAX)
)
AS
BEGIN
	IF(@ModulesIds IS NOT NULL AND LEN(TRIM(@ModulesIds)) > 0)
	BEGIN
		DECLARE @tblModuleIds TABLE (ModuleId varchar(100))
		INSERT INTO @tblModuleIds SELECT value  FROM STRING_SPLIT(@ModulesIds, '|')
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
CREATE OR ALTER PROCEDURE [dbo].sp_organizationRoles_search
(
	@OrganizationId uniqueidentifier,
	@PageIndex int = 0,
	@PageSize int = 10
)
AS
BEGIN
	If @PageIndex IS NULL
		SET @PageIndex = 0
	If @PageSize IS NULL
		SET @PageSize = 10

	SELECT COUNT(*) FROM OrganizationRoles  WHERE (@OrganizationId IS NULL OR OrganizationId = @OrganizationId) AND IsDeleted = 0

	SELECT * FROM OrganizationRoles WHERE (@OrganizationId IS NULL OR OrganizationId = @OrganizationId) AND IsDeleted = 0
	ORDER BY UpdatedDate DESC
	OFFSET @PageIndex ROWS
	FETCH NEXT @PageSize ROWS ONLY;

	SELECT OrganizationRolesMapping.FeatureId, OrganizationRolesMapping.OrganizationRoleId FROM OrganizationRolesMapping, Features 
		WHERE OrganizationRolesMapping.FeatureId = Features.Id
		AND OrganizationRolesMapping.OrganizationRoleId IN (SELECT Id FROM OrganizationRoles WHERE 
		(@OrganizationId IS NULL OR OrganizationId = @OrganizationId) AND IsDeleted = 0
			ORDER BY UpdatedDate DESC
			OFFSET @PageIndex ROWS
			FETCH NEXT @PageSize ROWS ONLY)
END
GO
--------------------------------------------------------------------
CREATE OR ALTER  PROCEDURE [dbo].sp_organizationRoles_getById
(
	@OrganizationRoleId uniqueidentifier
)
AS
BEGIN
	SELECT * FROM OrganizationRoles WHERE ID = @OrganizationRoleId AND IsDeleted = 0
	SELECT * FROM Features WHERE ID IN (SELECT FeatureId FROM OrganizationRolesMapping WHERE OrganizationRoleId = @OrganizationRoleId)
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].sp_organizationRoles_create
(
	@Id uniqueidentifier,
	@RoleCode nvarchar(100) = NULL,
	@RoleName nvarchar(MAX),
	@OrganizationId uniqueidentifier,
	@Description nvarchar(MAX),
	@CreatedDate datetime,
	@CreatedBy nvarchar(MAX),
	@UpdatedDate datetime,
	@UpdatedBy nvarchar(MAX),
	@FeatureIds nvarchar(MAX)
)
AS
BEGIN
	DECLARE @OrganizationRoleId uniqueidentifier

	INSERT INTO OrganizationRoles
	(Id, RoleCode, RoleName, OrganizationId, [Description], CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
	VALUES
	(@Id, @RoleCode, @RoleName, @OrganizationId, @Description, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)

	IF(@FeatureIds IS NOT NULL AND LEN(TRIM(@FeatureIds)) > 0)
	BEGIN
		DECLARE @tblFeatureId TABLE (FeatureId uniqueidentifier)
		INSERT INTO @tblFeatureId  SELECT value  FROM STRING_SPLIT(@FeatureIds, '|')
		INSERT INTO OrganizationRolesMapping (OrganizationRoleId, FeatureId) SELECT @Id, FeatureId FROM @tblFeatureId
	END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].sp_organizationRoles_update
(
	@Id uniqueidentifier,
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
	WHERE ID = @Id

	IF(@FeatureIds IS NOT NULL AND LEN(TRIM(@FeatureIds)) > 0)
	BEGIN
		DECLARE @tblFeatureId TABLE (FeatureId uniqueidentifier)
		INSERT INTO @tblFeatureId  SELECT value  FROM STRING_SPLIT(@FeatureIds, '|')

		DELETE FROM OrganizationRolesMapping WHERE OrganizationRoleId = @Id AND FeatureId NOT IN (SELECT FeatureId FROM @tblFeatureId)

		INSERT INTO OrganizationRolesMapping (OrganizationRoleId, FeatureId) SELECT @Id, FeatureId FROM @tblFeatureId
		WHERE FeatureId NOT IN (SELECT FeatureId FROM OrganizationRolesMapping WHERE OrganizationRoleId = @Id)
	END
	ELSE
	BEGIN
		DELETE FROM OrganizationRolesMapping WHERE OrganizationRoleId = @Id
	END

	SELECT * FROM OrganizationRoles WHERE ID = @Id
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].sp_organizationRoles_delete
(
	@RoleId uniqueidentifier
)
AS
BEGIN
	UPDATE OrganizationRoles SET IsDeleted = 1 WHERE ID = @RoleId
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_modules_seeding]
(
	@Modules ModuleType READONLY
)
AS
BEGIN
	INSERT INTO Modules (ModuleCode, ModuleName, IsActive)
	SELECT Code,[Name], 1 FROM @Modules WHERE Code NOT IN (SELECT ModuleCode FROM Modules)
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_features_seeding]
(
	@Features FeatureType READONLY
)
AS
BEGIN
	INSERT INTO Features (ModuleId, FeatureCode, FeatureName)
	SELECT Modules.ID, Code
	,[Name] FROM @Features B, Modules WHERE B.ModuleCode = Modules.ModuleCode
	AND B.Code NOT IN (SELECT FeatureCode FROM Features)
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_organizationRoles_bulkUpdate]
(
	@Roles OrganizationRoleType READONLY
)
AS
BEGIN
	DECLARE @RoleId uniqueidentifier
	DECLARE @FeaturesIds nvarchar(MAX)
	DECLARE RoleCursor CURSOR 
		FOR SELECT * FROM @Roles
	OPEN RoleCursor
	FETCH NEXT FROM RoleCursor INTO @RoleId, @FeaturesIds
	
	WHILE @@FETCH_STATUS = 0  
    BEGIN
		IF(@FeaturesIds IS NULL OR LEN(TRIM(@FeaturesIds)) = 0)
		BEGIN
			DELETE OrganizationRolesMapping WHERE OrganizationRoleId = @RoleId
		END
		ELSE
		BEGIN
			DECLARE @tblFeatureIds TABLE (FeatureId uniqueidentifier)
			INSERT INTO @tblFeatureIds select CAST(value as uniqueidentifier) from string_split(@FeaturesIds, '|');
		
			DELETE OrganizationRolesMapping WHERE OrganizationRoleId = @RoleId AND FeatureId NOT IN (SELECT FeatureId FROM @tblFeatureIds)

			INSERT INTO OrganizationRolesMapping
			(OrganizationRoleId, FeatureId) SELECT @RoleId, FeatureId FROM @tblFeatureIds
			WHERE FeatureId NOT IN (SELECT FeatureId FROM OrganizationRolesMapping WHERE OrganizationRoleId = @RoleId)
		END

        FETCH NEXT FROM RoleCursor;  
    END
	CLOSE RoleCursor
	DEALLOCATE RoleCursor;
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_clean
AS
BEGIN
	DELETE FROM OrganizationRolesMapping
	DELETE FROM OrganizationUserRoles
	DELETE FROM OrganizationRoles
	DELETE FROM Features
END
GO