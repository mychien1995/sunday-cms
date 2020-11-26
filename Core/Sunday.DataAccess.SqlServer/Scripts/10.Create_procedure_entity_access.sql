CREATE OR ALTER PROCEDURE dbo.sp_entityAccess_getByOrganization
@OrganizationId uniqueidentifier,
@EntityType varchar(50)
AS
BEGIN
	SELECT * FROM EntityAccesses WHERE OrganizationId = @OrganizationId AND EntityType = @EntityType
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_entityAccess_getByEntity
@EntityId uniqueidentifier,
@EntityType varchar(50)
AS
BEGIN
	SELECT * FROM EntityAccesses WHERE EntityId = @EntityId AND EntityType = @EntityType
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_entityAccess_getByEntities
@EntityIds nvarchar(MAX),
@EntityType varchar(50)
AS
BEGIN
	DECLARE  @tblIds TABLE (EntityId uniqueidentifier)
	IF @EntityIds IS NOT NULL AND LEN(TRIM(@EntityIds)) > 0
	INSERT INTO @tblIds SELECT Cast(value as uniqueidentifier) FROM STRING_SPLIT(@EntityIds, '|')
	SELECT * FROM EntityAccesses WHERE EntityId IN (SELECT EntityId FROM @tblIds) AND EntityType = @EntityType
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_entityAccess_save
@EntityId uniqueidentifier,
@EntityType varchar(50),
@EntityAccess EntityAccessOrganizationType READONLY
AS
BEGIN
	DELETE FROM EntityAccesses WHERE EntityId = @EntityId AND EntityType = @EntityType
	INSERT INTO EntityAccesses (EntityId, EntityType, OrganizationId, WebsiteIds, OrganizationRoleIds)
	SELECT @EntityId, @EntityType, OrganizationId, WebsiteIds, OrganizationRoleIds FROM @EntityAccess
END
GO
--------------------------------------------------------------------