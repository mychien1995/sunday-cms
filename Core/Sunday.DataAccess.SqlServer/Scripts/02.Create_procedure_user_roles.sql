CREATE OR ALTER PROCEDURE [dbo].[sp_database_seeding]
(
	@PasswordHash nvarchar(MAX),
	@SecurityStamp nvarchar(500),
	@RoleType RoleType READONLY
)
AS
BEGIN
	DECLARE @UserExist bit = 1
	DECLARE @UserId uniqueidentifier
	SET @UserId = NEWID()
	DECLARE @RoleId integer
	SET @RoleId = (SELECT TOP 1 Id FROM @RoleType WHERE Code = 'SA')
	IF NOT EXISTS (select 1 from dbo.Users WHERE Username = 'admin' and Domain = 'CMS')
	BEGIN
		SET @UserExist = 0
		INSERT INTO Users(Id, Username, FullName, Domain, EmailConfirmed, CreatedBy, UpdatedBy, SecurityStamp, PasswordHash)
		VALUES (@UserId, 'admin', N'System Admin', 'CMS', 1, 'System' , 'System', @SecurityStamp, @PasswordHash)
	END

	IF NOT EXISTS (select TOP 1 * from dbo.Roles)
	BEGIN
		INSERT INTO dbo.Roles
		SELECT * FROM @RoleType
	END

	IF (@UserExist = 0)
	BEGIN
		INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)
	END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_findbyusername]
(
	@Username nvarchar(MAX)
)
AS
BEGIN
	SELECT * FROM [Users] WHERE Username = @Username AND IsDeleted = 0;
	SELECT * FROM [Roles] WHERE ID IN (SELECT RoleId FROM UserRoles WHERE UserId IN (SELECT ID FROM [Users] WHERE Username = @Username  AND IsDeleted = 0));
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_getById]
(
	@UserId uniqueidentifier
)
AS
BEGIN
	SELECT * FROM [Users] WHERE ID = @UserId AND IsDeleted = 0;
	SELECT * FROM [Roles] WHERE ID IN (SELECT RoleId FROM UserRoles WHERE UserId = @UserId);
	SELECT * FROM [OrganizationUsers] WHERE UserId = @UserId;
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_search]
(
	@PageIndex int = 0,
	@PageSize int = 10,
	@ExcludeIds nvarchar(MAX) = '',
	@IncludeIds nvarchar(MAX) = '',
	@OrganizationIds nvarchar(MAX) = '',
	@RoleIds nvarchar(MAX) = '',
	@Text nvarchar(MAX) = '',
	@Username nvarchar(MAX) = '',
	@Email nvarchar(MAX) = '',
	@SortBy nvarchar(MAX) = 'UpdatedDate',
	@SortDirection nvarchar(MAX) = 'DESC'
)
AS
BEGIN
	IF @PageIndex IS NULL
		SET @PageIndex = 0

	IF @PageSize IS NULL
		SET @PageSize = 10

	SET @PageIndex = @PageIndex * @PageSize

	IF @SortBy IS NULL
		SET @SortBy = 'UpdatedDate'

	DECLARE @WhereClause nvarchar(MAX);
	SET @WhereClause = ' IsDeleted = 0 ';

	IF(@ExcludeIds IS NOT NULL AND LEN(TRIM(@ExcludeIds)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' ID NOT IN (' + dbo.ParseIdList(@ExcludeIds) + ') ';
	END

	IF(@IncludeIds IS NOT NULL AND LEN(TRIM(@IncludeIds)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' ID IN (' + dbo.ParseIdList(@IncludeIds) + ') ';
	END

	IF(@Text IS NOT NULL AND LEN(TRIM(@Text)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' (Username LIKE ''%'' + @Text + ''%'' OR FullName LIKE ''%'' + @Text + ''%'' OR Email LIKE ''%'' + @Text + ''%'') ' ;
	END

	IF(@RoleIds IS NOT NULL AND LEN(TRIM(@RoleIds)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' ID IN (SELECT UserId FROM UserRoles WHERE RoleId IN (' + dbo.ParseIdList(@RoleIds) + ')) ';
	END

	IF(@OrganizationIds IS NOT NULL AND LEN(TRIM(@OrganizationIds)) > 0)
	BEGIN
		IF LEN(TRIM(@WhereClause)) > 0
			SET @WhereClause = @WhereClause + ' AND ';
		SET @WhereClause = @WhereClause + ' ID IN (SELECT UserId FROM OrganizationUsers WHERE OrganizationId IN (' + dbo.ParseIdList(@OrganizationIds) + ')) ';
	END

	IF(LEN(TRIM(@WhereClause)) > 0)
		SET @WhereClause = ' WHERE ' + @WhereClause
	DECLARE @CountQuery nvarchar(MAX);
	SET @CountQuery = 'SELECT COUNT(*) FROM [Users] ' + @WhereClause
	
	DECLARE @DataQuery nvarchar(MAX);
	SET @DataQuery = 'SELECT * FROM [Users] ' + @WhereClause  + ' ORDER BY ' + @SortBy + ' ' + @SortDirection
	+ ' OFFSET ' + CAST(@PageIndex AS VARCHAR(100)) + ' ROWS FETCH NEXT '+ CAST(@PageSize AS VARCHAR(100)) +' ROWS ONLY'

	PRINT @DataQuery

	exec sp_executesql @CountQuery, 
	N'@Text nvarchar(MAX)',
	@Text

	exec sp_executesql @DataQuery, 
	N'@Text nvarchar(MAX)',
	@Text
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_insert]
(
	@Id uniqueidentifier,
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
	@PasswordHash nvarchar(1000),
	@RoleIds nvarchar(MAX),
	@OrganizationRoles OrganizationUserRoleType READONLY,
	@Organizations OrganizationUserType READONLY
)
AS
BEGIN
	INSERT INTO Users(Id, Username, FullName, Email, Phone, Domain, IsActive, EmailConfirmed, CreatedBy, UpdatedBy, SecurityStamp, PasswordHash)
	VALUES (@Id, @UserName, @Fullname, @Email, @Phone , @Domain, @IsActive, @EmailConfirmed, @CreatedBy, @UpdatedBy, @SecurityStamp, @PasswordHash)

	DECLARE @tblRoleIds TABLE (RoleId varchar(100))
	INSERT INTO @tblRoleIds SELECT value  FROM STRING_SPLIT(@RoleIds, '|')
	INSERT INTO UserRoles (UserId, RoleId) SELECT @Id, RoleId FROM @tblRoleIds

	INSERT INTO OrganizationUsers(UserId, OrganizationId, IsActive) SELECT @Id, OrganizationId, IsActive FROM @Organizations


	DECLARE @OrganizationId uniqueidentifier
	DECLARE @OrganizationRolesId nvarchar(MAX)
	DECLARE OrganizationCursor CURSOR FOR SELECT OrganizationId, OrganizationRolesId FROM @OrganizationRoles
	OPEN OrganizationCursor
	FETCH NEXT FROM OrganizationCursor INTO @OrganizationId, @OrganizationRolesId
	WHILE @@FETCH_STATUS = 0  
    BEGIN
		DECLARE @tblOrgRole TABLE (OrgRoleId varchar(100))
		INSERT INTO @tblOrgRole SELECT value  FROM STRING_SPLIT(@OrganizationRolesId, '|')
		
		DECLARE @OrganizationUserId uniqueidentifier
		SET @OrganizationUserId = (SELECT TOP 1 ID FROM OrganizationUsers WHERE UserId = @Id AND OrganizationId = @OrganizationId)

		INSERT INTO OrganizationUserRoles (OrganizationUserId, OrganizationRoleId)
		SELECT @OrganizationUserId, OrgRoleId FROM @tblOrgRole
		FETCH NEXT FROM OrganizationCursor INTO @OrganizationUserId, @OrganizationId
	END

	CLOSE OrganizationCursor
	DEALLOCATE OrganizationCursor;
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_roles_getAll]
AS
BEGIN
	SELECT * FROM [Roles];
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_getById_withOptions]
(
	@UserId uniqueidentifier,
	@FetchRoles bit = 1,
	@FetchOrganizations bit = 1,
	@FetchVirtualRoles bit = 1
)
AS
BEGIN
	SELECT * FROM Users WHERE ID = @UserId AND IsDeleted = 0;
	IF @FetchRoles = 1
	BEGIN
		SELECT * FROM Roles WHERE ID IN (SELECT RoleId FROM UserRoles WHERE UserId = @UserId)
	END
	IF @FetchOrganizations = 1
	BEGIN
		SELECT OrganizationUsers.ID, OrganizationUsers.OrganizationId, OrganizationUsers.UserId,
		OrganizationUsers.IsActive, Organizations.OrganizationName
		FROM OrganizationUsers, Organizations
			WHERE OrganizationUsers.UserId = @UserId
			AND Organizations.ID = OrganizationUsers.OrganizationId
	END
	IF @FetchVirtualRoles = 1
	BEGIN
		SELECT ID, RoleName FROM OrganizationRoles WHERE ID IN 
		(SELECT OrganizationRoleId FROM OrganizationUserRoles WHERE OrganizationUserId IN 
			(SELECT ID FROM OrganizationUsers WHERE UserId = @UserId AND IsActive = 1)
		)
	END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_update]
(
	@ID uniqueidentifier,
	@Fullname nvarchar(500),
	@Email nvarchar(500) = NULL,
	@Phone nvarchar(500) = NULL,
	@IsActive bit = 1,
	@UpdatedBy nvarchar(500),
	@UpdatedDate datetime,
	@AvatarBlobUri nvarchar(MAX) = NULL,
	@RoleIds nvarchar(MAX) = NULL,
	@OrganizationRoles OrganizationUserRoleType READONLY,
	@Organizations OrganizationUserType READONLY
)
AS
BEGIN
	IF(@UpdatedDate IS NULL)
		SET @UpdatedDate = GETDATE()

	UPDATE [Users] SET FullName = @Fullname, Email = @Email, Phone = @Phone, IsActive = @IsActive, UpdatedBy = @UpdatedBy,
	UpdatedDate = @UpdatedDate, AvatarBlobUri = @AvatarBlobUri
	WHERE ID = @ID
	SELECT @ID
	
	IF(@RoleIds IS NOT NULL AND LEN(TRIM(@RoleIds)) > 0)
	BEGIN
		DECLARE @tblRoleIds TABLE (RoleId varchar(100))
		INSERT INTO @tblRoleIds SELECT value  FROM STRING_SPLIT(@RoleIds, '|')
		DELETE FROM UserRoles WHERE UserId = @ID
		INSERT INTO UserRoles (UserId, RoleId) SELECT @ID, RoleId FROM @tblRoleIds
	END

	BEGIN
		DELETE FROM OrganizationUsers WHERE UserId = @ID AND OrganizationId NOT IN (SELECT OrganizationId FROM @Organizations)

		INSERT INTO OrganizationUsers(UserId, OrganizationId, IsActive) SELECT @ID, OrganizationId, IsActive FROM 
			(SELECT OrganizationId, IsActive FROM @Organizations WHERE OrganizationId NOT IN 
				(SELECT OrganizationId FROM OrganizationUsers WHERE UserId = @ID)) B

		MERGE INTO OrganizationUsers AS tgt
			USING @Organizations AS src
		ON tgt.OrganizationId = src.OrganizationId AND tgt.UserId = @ID
		WHEN MATCHED THEN
        UPDATE 
            SET tgt.IsActive = src.IsActive;
	END

	BEGIN
		DECLARE @OrganizationId uniqueidentifier
		DECLARE @OrganizationRolesId nvarchar(MAX)
		DECLARE OrganizationCursor CURSOR FOR SELECT OrganizationId, OrganizationRolesId FROM @OrganizationRoles
		OPEN OrganizationCursor
		FETCH NEXT FROM OrganizationCursor INTO @OrganizationId, @OrganizationRolesId
		WHILE @@FETCH_STATUS = 0  
		BEGIN
			DECLARE @tblOrgRole TABLE (OrgRoleId varchar(100))
			INSERT INTO @tblOrgRole SELECT value  FROM STRING_SPLIT(@OrganizationRolesId, '|')
		
			DECLARE @OrganizationUserId uniqueidentifier
			SET @OrganizationUserId = (SELECT TOP 1 ID FROM OrganizationUsers WHERE UserId = @ID AND OrganizationId = @OrganizationId)

			DELETE FROM OrganizationUserRoles WHERE OrganizationUserId = @OrganizationUserId 
			AND OrganizationRoleId IN (SELECT ID FROM OrganizationRoles WHERE OrganizationId = @OrganizationId)
			AND OrganizationRoleId NOT IN (SELECT OrgRoleId FROM @tblOrgRole)

			INSERT INTO OrganizationUserRoles (OrganizationUserId, OrganizationRoleId)
			SELECT @OrganizationUserId, OrgRoleId FROM @tblOrgRole
			WHERE OrgRoleId NOT IN (SELECT OrganizationRoleId FROM OrganizationUserRoles WHERE OrganizationUserId = @OrganizationUserId)

			FETCH NEXT FROM OrganizationCursor INTO @OrganizationUserId, @OrganizationId
		END

		CLOSE OrganizationCursor
		DEALLOCATE OrganizationCursor;
	END
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_updateAvatar]
(
	@UserId uniqueidentifier,
	@BlobUri nvarchar(MAX)
)
AS
BEGIN
	UPDATE Users SET AvatarBlobUri = @BlobUri WHERE ID = @UserId
	SELECT * FROM Users WHERE ID = @UserId
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_delete]
(
	@UserId uniqueidentifier
)
AS
BEGIN
	UPDATE [Users] Set IsDeleted = 1 WHERE ID = @UserId
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_users_fetchRoles
(
	@UserIds nvarchar(MAX)
)
AS
BEGIN
	IF(@UserIds IS NULL OR LEN(TRIM(@UserIds)) = 0)
	BEGIN
		SELECT * FROM Roles WHERE 1 = 2
	END
	DECLARE @tblUserIds TABLE (ID varchar(100))
	INSERT INTO @tblUserIds SELECT value  FROM STRING_SPLIT(@UserIds, '|')

	SELECT UserId, RoleId, Code, RoleName 
	FROM UserRoles, Roles 
	WHERE UserId IN (SELECT ID FROM @tblUserIds)
	AND UserRoles.RoleId = Roles.ID

END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_users_activate
(
	@UserId uniqueidentifier
)
AS
BEGIN
	UPDATE Users SET IsActive = 1 WHERE ID = @UserId
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_users_deactivate
(
	@UserId uniqueidentifier
)
AS
BEGIN
	UPDATE Users SET IsActive = 0 WHERE ID = @UserId
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_users_changePassword
(
	@UserId uniqueidentifier,
	@SecurityHash nvarchar(MAX),
	@PasswordHash nvarchar(MAX)
)
AS
BEGIN
	UPDATE Users SET SecurityStamp = @SecurityHash, PasswordHash = @PasswordHash WHERE ID = @UserId
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_roles_getById
(
	@RoleId integer
)
AS
BEGIN
	SELECT * FROM Roles WHERE ID = @RoleId
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_fetchOrganizationRoles]
(
	@UserIds nvarchar(MAX)
)
AS
BEGIN
	IF(@UserIds IS NULL OR LEN(TRIM(@UserIds)) = 0)
	BEGIN
		SELECT * FROM OrganizationRoles WHERE 1 = 2
	END
	DECLARE @tblUserIds TABLE (ID varchar(100))
	INSERT INTO @tblUserIds SELECT value  FROM STRING_SPLIT(@UserIds, '|')

	SELECT UserId, OrganizationRoles.ID, OrganizationRoles.RoleCode, OrganizationRoles.RoleName 
	FROM OrganizationRoles, OrganizationUserRoles, OrganizationUsers 
	WHERE OrganizationUsers.UserId IN (SELECT ID FROM @tblUserIds)
	AND OrganizationUserRoles.OrganizationUserId = OrganizationUsers.ID
	AND OrganizationUserRoles.OrganizationRoleId = OrganizationRoles.ID

END
GO