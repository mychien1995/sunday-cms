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
	SELECT * FROM Users WHERE ID = @UserId AND IsDeleted = 0;
	SELECT Id, RoleName, Code FROM [Roles] WHERE Id IN (SELECT RoleId FROM [UserRoles] WHERE UserId = @UserId);
	SELECT OrganizationId, IsActive FROM [OrganizationUsers] WHERE UserId = @UserId;
	SELECT OrganizationRoleId, [OrganizationRoles].RoleName FROM [OrganizationUserRoles], [OrganizationRoles]
	WHERE OrganizationUserId IN (SELECT Id FROM [OrganizationUsers] WHERE UserId = @UserId)
	AND [OrganizationUserRoles].OrganizationRoleId = [OrganizationRoles].Id;
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
	@SortDirection nvarchar(MAX) = 'DESC',
	@IncludeRoles bit = 1,
	@IncludeOrganizations bit = 1,
	@IncludeVirtualRoles bit = 1
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
		SET @WhereClause = @WhereClause + ' AND ID NOT IN (' + dbo.ParseIdList(@ExcludeIds) + ') ';
	IF(@IncludeIds IS NOT NULL AND LEN(TRIM(@IncludeIds)) > 0)
		SET @WhereClause = @WhereClause + ' AND ID IN (' + dbo.ParseIdList(@IncludeIds) + ') ';
	IF(@Text IS NOT NULL AND LEN(TRIM(@Text)) > 0)
		SET @WhereClause = @WhereClause + ' AND (Username LIKE ''%'' + @Text + ''%'' OR FullName LIKE ''%'' + @Text + ''%'' OR Email LIKE ''%'' + @Text + ''%'') ' ;
	IF(@Email IS NOT NULL AND LEN(TRIM(@Email)) > 0)
		SET @WhereClause = @WhereClause + ' AND (Email = @Email) ' ;
	IF(@Username IS NOT NULL AND LEN(TRIM(@Username)) > 0)
		SET @WhereClause = @WhereClause + ' AND (Username = @Username) ' ;
	IF(@RoleIds IS NOT NULL AND LEN(TRIM(@RoleIds)) > 0)
		SET @WhereClause = @WhereClause + ' AND ID IN (SELECT UserId FROM UserRoles WHERE RoleId IN (' + dbo.ParseIdList(@RoleIds) + ')) ';
	IF(@OrganizationIds IS NOT NULL AND LEN(TRIM(@OrganizationIds)) > 0)
		SET @WhereClause = @WhereClause + ' AND ID IN (SELECT UserId FROM OrganizationUsers WHERE OrganizationId IN (' + dbo.ParseIdList(@OrganizationIds) + ')) ';
	IF(LEN(TRIM(@WhereClause)) > 0)
		SET @WhereClause = ' WHERE ' + @WhereClause
	DECLARE @CountQuery nvarchar(MAX);
	SET @CountQuery = 'SELECT COUNT(*) FROM [Users] ' + @WhereClause
	--select total--
	exec sp_executesql @CountQuery, N'@Text nvarchar(MAX), @Email nvarchar(MAX), @Username nvarchar(MAX)', @Text, @Email, @Username
	
	SELECT [Id],[Domain],[Username],[FullName],[Email],[Phone],[AvatarBlobUri],[IsActive]
		,[IsLockedOut],[EmailConfirmed],[CreatedDate],[CreatedBy],[UpdatedDate],[UpdatedBy] into  #tmpUsers 
		FROM [Users] WHERE 1 = 2;
	DELETE FROM #tmpUsers
	DECLARE @DataQuery nvarchar(MAX);
	SET @DataQuery = 'SELECT [Id],[Domain],[Username],[FullName],[Email],[Phone],[AvatarBlobUri],[IsActive]
		,[IsLockedOut],[EmailConfirmed],[CreatedDate],[CreatedBy],[UpdatedDate],[UpdatedBy]
		FROM [Users] ' + @WhereClause  + ' ORDER BY ' + @SortBy + ' ' + @SortDirection
	+ ' OFFSET ' + CAST(@PageIndex AS VARCHAR(100)) + ' ROWS FETCH NEXT '+ CAST(@PageSize AS VARCHAR(100)) +' ROWS ONLY '
	PRINT @DataQuery
	INSERT INTO #tmpUsers exec sp_executesql @DataQuery, N'@Text nvarchar(MAX), @Email nvarchar(MAX), @Username nvarchar(MAX)', @Text, @Email, @Username
	
	--select users---
	SELECT * FROM #tmpUsers;
	--select system role--
	IF (@IncludeRoles = 1)
		SELECT [UserRoles].UserId, [UserRoles].RoleId, RoleName, Code FROM [Roles], [UserRoles] 
			WHERE [UserRoles].RoleId = [Roles].Id
			AND [UserRoles].UserId IN (SELECT Id FROM #tmpUsers);

	--select organizations--
	IF (@IncludeOrganizations = 1)
		SELECT * FROM [OrganizationUsers]
			WHERE UserId IN (SELECT Id FROM #tmpUsers);

	--select organizations roles--
	IF(@IncludeVirtualRoles = 1)
		SELECT [OrganizationUserRoles].OrganizationUserId,
			OrganizationRoleId, [OrganizationRoles].RoleName FROM [OrganizationUserRoles], [OrganizationRoles]
			WHERE OrganizationUserId IN (SELECT Id FROM [OrganizationUsers] WHERE UserId IN (SELECT Id FROM #tmpUsers))
			AND [OrganizationUserRoles].OrganizationRoleId = [OrganizationRoles].Id;
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
	@Organizations OrganizationUserRoleType READONLY
)
AS
BEGIN
	INSERT INTO Users(Id, Username, FullName, Email, Phone, Domain, IsActive, EmailConfirmed, CreatedBy, UpdatedBy, SecurityStamp, PasswordHash)
	VALUES (@Id, @UserName, @Fullname, @Email, @Phone , @Domain, @IsActive, @EmailConfirmed, @CreatedBy, @UpdatedBy, @SecurityStamp, @PasswordHash)

	DECLARE @tblRoleIds TABLE (RoleId varchar(100))
	INSERT INTO @tblRoleIds SELECT value  FROM STRING_SPLIT(@RoleIds, '|')
	INSERT INTO UserRoles (UserId, RoleId) SELECT @Id, RoleId FROM @tblRoleIds

	DECLARE @OrganizationId uniqueidentifier
	DECLARE @OrganizationRolesId nvarchar(MAX)
	DECLARE @UserActive bit
	DECLARE OrganizationCursor CURSOR FOR SELECT OrganizationId, IsActive, OrganizationRolesId FROM @Organizations
	OPEN OrganizationCursor
	FETCH NEXT FROM OrganizationCursor INTO @OrganizationId, @UserActive, @OrganizationRolesId
	WHILE @@FETCH_STATUS = 0  
    BEGIN
		DECLARE @OrganizationUserId uniqueidentifier;
		SET @OrganizationUserId = NEWID()
		INSERT INTO OrganizationUsers (Id, UserId, OrganizationId, IsActive) VALUES (@OrganizationUserId, @Id, @OrganizationId, @UserActive)
		IF @OrganizationRolesId IS NOT NULL AND LEN(TRIM(@OrganizationRolesId)) > 0
		BEGIN
			DECLARE @tblOrgRole TABLE (OrgRoleId uniqueidentifier)
			INSERT INTO @tblOrgRole SELECT Cast(value as uniqueidentifier) FROM STRING_SPLIT(@OrganizationRolesId, '|')
			INSERT INTO OrganizationUserRoles (OrganizationUserId, OrganizationRoleId)
			SELECT @OrganizationUserId, OrgRoleId FROM @tblOrgRole
		END
		FETCH NEXT FROM OrganizationCursor INTO @OrganizationId, @UserActive, @OrganizationRolesId
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
	@UserId uniqueidentifier
)
AS
BEGIN
	SELECT * FROM Users WHERE ID = @UserId AND IsDeleted = 0;
	SELECT Id, RoleName, Code FROM [Roles] WHERE Id IN (SELECT RoleId FROM [UserRoles] WHERE UserId = @UserId);
	SELECT OrganizationId, [OrganizationUsers].IsActive FROM [OrganizationUsers] WHERE UserId = @UserId;
	SELECT [OrganizationRoles].Id, [OrganizationRoles].RoleName FROM [OrganizationUserRoles], [OrganizationRoles]
		WHERE OrganizationUserId IN (SELECT Id FROM [OrganizationUsers] WHERE UserId = @UserId)
		AND [OrganizationUserRoles].OrganizationRoleId = [OrganizationRoles].Id;
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE [dbo].[sp_users_update]
(
	@Id uniqueidentifier,
	@Fullname nvarchar(500),
	@Email nvarchar(500) = NULL,
	@Phone nvarchar(500) = NULL,
	@IsActive bit = 1,
	@UpdatedBy nvarchar(500),
	@UpdatedDate datetime,
	@AvatarBlobUri nvarchar(MAX) = NULL,
	@RoleIds nvarchar(MAX) = NULL,
	@Organizations OrganizationUserRoleType READONLY
)
AS
BEGIN
	IF(@UpdatedDate IS NULL)
		SET @UpdatedDate = GETDATE()

	UPDATE [Users] SET FullName = @Fullname, Email = @Email, Phone = @Phone, IsActive = @IsActive, UpdatedBy = @UpdatedBy,
	UpdatedDate = @UpdatedDate, AvatarBlobUri = @AvatarBlobUri
	WHERE Id = @Id

	IF(@RoleIds IS NOT NULL AND LEN(TRIM(@RoleIds)) > 0)
	BEGIN
		DECLARE @tblRoleIds TABLE (RoleId varchar(100))
		INSERT INTO @tblRoleIds SELECT value  FROM STRING_SPLIT(@RoleIds, '|')
		DELETE FROM UserRoles WHERE UserId = @ID
		INSERT INTO UserRoles (UserId, RoleId) SELECT @ID, RoleId FROM @tblRoleIds
	END

	BEGIN
		--- Clear role from old organization ---
		DELETE FROM OrganizationUserRoles WHERE OrganizationUserId IN 
		(SELECT Id FROM OrganizationUsers WHERE UserId = @Id AND OrganizationId NOT IN (SELECT OrganizationId FROM @Organizations))

		--- Clear old organization ---
		DELETE FROM OrganizationUsers WHERE UserId = @Id AND OrganizationId NOT IN (SELECT OrganizationId FROM @Organizations)

		DECLARE @OrganizationId uniqueidentifier
		DECLARE @OrganizationRolesId nvarchar(MAX)
		DECLARE @UserActive nvarchar(MAX)
		DECLARE OrganizationCursor CURSOR FOR SELECT OrganizationId, OrganizationRolesId, IsActive FROM @Organizations
		OPEN OrganizationCursor
		FETCH NEXT FROM OrganizationCursor INTO @OrganizationId, @OrganizationRolesId, @UserActive
		WHILE @@FETCH_STATUS = 0  
		BEGIN
			DECLARE @tblOrgRole TABLE (OrgRoleId varchar(100))
			DECLARE @OrganizationUserId uniqueidentifier;
			SELECT @OrganizationUserId = Id FROM OrganizationUsers WHERE OrganizationId = @OrganizationId AND UserId = @Id
			-- If user not in organization --
			IF @OrganizationUserId IS NULL
			BEGIN
				SET @OrganizationUserId = NEWID()
				INSERT INTO OrganizationUsers (Id, UserId, OrganizationId, IsActive) VALUES (@OrganizationUserId, @Id, @OrganizationId, @UserActive)
				IF @OrganizationRolesId IS NOT NULL AND LEN(TRIM(@OrganizationRolesId)) > 0
				BEGIN
					INSERT INTO @tblOrgRole SELECT value  FROM STRING_SPLIT(@OrganizationRolesId, '|')
					INSERT INTO OrganizationUserRoles (OrganizationUserId, OrganizationRoleId) SELECT @OrganizationUserId, OrgRoleId FROM @tblOrgRole
				END
			END
			ELSE
			-- If user already in organization --
			BEGIN
				UPDATE OrganizationUsers SET IsActive = @UserActive WHERE Id = @OrganizationUserId;
				IF @OrganizationRolesId IS NOT NULL AND LEN(TRIM(@OrganizationRolesId)) > 0
				BEGIN
					INSERT INTO @tblOrgRole SELECT value  FROM STRING_SPLIT(@OrganizationRolesId, '|')
					DELETE FROM OrganizationUserRoles WHERE OrganizationUserId = @OrganizationUserId AND OrganizationRoleId NOT IN (SELECT OrgRoleId FROM @tblOrgRole)
					INSERT INTO OrganizationUserRoles (OrganizationUserId, OrganizationRoleId) SELECT @OrganizationUserId, OrgRoleId FROM @tblOrgRole
					WHERE OrgRoleId NOT IN (SELECT OrganizationRoleId FROM OrganizationUserRoles WHERE OrganizationUserId = @OrganizationUserId)
				END
			END
			DELETE FROM @tblOrgRole
			FETCH NEXT FROM OrganizationCursor INTO @OrganizationUserId, @OrganizationId, @UserActive
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