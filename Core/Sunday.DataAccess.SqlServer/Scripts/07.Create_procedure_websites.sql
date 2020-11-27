﻿CREATE OR ALTER PROCEDURE sp_websites_insert
(
	 @Id uniqueidentifier
	,@WebsiteName nvarchar(1000)
	,@HostNames nvarchar(max)
	,@OrganizationId uniqueidentifier
	,@LayoutId uniqueidentifier
	,@PageDesignMappings nvarchar(MAX)
	,@IsActive bit
	,@CreatedDate datetime
	,@CreatedBy nvarchar(500)
	,@UpdatedDate datetime
	,@UpdatedBy nvarchar(500)
)
AS
BEGIN
	INSERT INTO [dbo].[Websites]
           ([Id]
           ,[WebsiteName]
           ,[HostNames]
           ,[OrganizationId]
           ,[LayoutId]
		   ,[PageDesignMappings]
           ,[IsActive]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[IsDeleted])
     VALUES
           ( @Id
			,@WebsiteName
			,@HostNames
			,@OrganizationId
			,@LayoutId
			,@PageDesignMappings
			,@IsActive
			,@CreatedDate
			,@CreatedBy
			,@UpdatedDate
			,@UpdatedBy
			,0)
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_websites_update
(
	 @Id uniqueidentifier
	,@WebsiteName nvarchar(1000)
	,@HostNames nvarchar(max)
	,@PageDesignMappings nvarchar(MAX)
	,@LayoutId uniqueidentifier
	,@IsActive bit
	,@UpdatedDate datetime
	,@UpdatedBy nvarchar(500)
)
AS
BEGIN
	UPDATE [dbo].[Websites] SET WebsiteName = @WebsiteName, HostNames = @HostNames,
	LayoutId = @LayoutId, IsActive = @IsActive, UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy,
	PageDesignMappings = @PageDesignMappings
	WHERE Id = @Id
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_websites_delete
(
	 @Id uniqueidentifier
)
AS
BEGIN
	UPDATE [dbo].[Websites] SET IsDeleted = 1 WHERE Id = @Id
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_websites_search
(
	@OrganizationId uniqueidentifier,
	@PageSize int = 10,
	@PageIndex int = 0
)
AS
BEGIN
	IF(@PageSize IS NULL) SET @PageSize = 10
	IF(@PageIndex IS NULL) SET @PageIndex = 0
	SELECT COUNT(*) FROM dbo.Websites WHERE IsDeleted = 0 AND (@OrganizationId IS NULL OR OrganizationId = @OrganizationId)
	SELECT * FROM dbo.Websites WHERE IsDeleted = 0 AND (@OrganizationId IS NULL OR OrganizationId = @OrganizationId)
	ORDER BY UpdatedDate DESC
	OFFSET @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_websites_getById
(
	@Id uniqueidentifier
)
AS
BEGIN
	SELECT * FROM dbo.Websites WHERE Id = @Id AND IsDeleted = 0
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_websites_getByHostName
(
	@HostName nvarchar(500)
)
AS
BEGIN
	SELECT * FROM Websites WHERE IsDeleted = 0 AND IsActive = 1 AND (HostNames =  @HostName OR HostNames LIKE '%|' + @HostName + '|%'
	OR HostNames LIKE '%|' + @HostName)
END
GO