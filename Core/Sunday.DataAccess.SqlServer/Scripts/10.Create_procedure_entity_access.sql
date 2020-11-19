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