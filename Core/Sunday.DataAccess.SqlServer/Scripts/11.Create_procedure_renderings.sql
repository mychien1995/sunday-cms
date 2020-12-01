CREATE OR ALTER PROCEDURE dbo.sp_renderings_search
(
	@Text nvarchar(1000),
	@IsPageRendering bit,
	@PageSize int,
	@PageIndex int
)
AS
BEGIN
	IF @PageSize IS NULL
		SET @PageSize = 100000
	IF @PageIndex IS NULL
		SET @PageIndex = 0

	DECLARE @Query nvarchar(max)
	SET @Query = 'IsDeleted = 0 '
	IF @Text IS NOT NULL AND LEN(TRIM(@Text)) > 0
		SET @Query = @Query + ' AND (RenderingName LIKE ''%' + @Text + '%'' OR cast(Id as varchar(100)) = '''+@Text+''')'
	IF @IsPageRendering  = 1
		SET @Query = @Query + ' AND RenderingType = ''PageRendering'' '
	IF @IsPageRendering  = 0
		SET @Query = @Query + ' AND RenderingType <> ''PageRendering'' '

	DECLARE @CountQuery nvarchar(max)
	SET @CountQuery = 'SELECT COUNT(*) FROM dbo.Renderings WHERE ' + @Query
	DECLARE @SearchQuery nvarchar(max)
	SET @SearchQuery = 'SELECT * FROM dbo.Renderings  WHERE ' + @Query + 'ORDER BY UpdatedDate DESC
	OFFSET @PageIndex ROWS FETCH NEXT @PageSize ROWS ONLY';
	exec sp_executesql @CountQuery, N'@IsPageRendering bit', @IsPageRendering
	exec sp_executesql @SearchQuery, N'@IsPageRendering bit, @PageIndex int, @PageSize int', @IsPageRendering, @PageIndex, @PageSize
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_renderings_getById
(
	@Id uniqueidentifier
)
AS
BEGIN
	SELECT * FROM Renderings WHERE Id = @Id AND IsDeleted = 0
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_renderings_createOrUpdate
	@Id uniqueidentifier,
	@RenderingName nvarchar(1000),
	@RenderingType nvarchar(1000),
	@Properties nvarchar(max),
	@IsPageRendering bit,
	@IsRequireDatasource bit,
	@DatasourceLocation varchar(max),
	@DatasourceTemplate uniqueidentifier,
	@CreatedDate datetime,
	@CreatedBy nvarchar(500),
	@UpdatedDate datetime,
	@UpdatedBy nvarchar(500),
	@IsDeleted bit
AS
BEGIN
	DECLARE @Exist int
	SET @Exist = (SELECT COUNT(1) FROM Renderings WHERE Id = @Id)
	IF @Exist = 0
	BEGIN
		INSERT INTO [dbo].[Renderings]
           ([Id]
           ,[RenderingName]
           ,[RenderingType]
           ,[Properties]
           ,[IsPageRendering]
           ,[IsRequireDatasource]
           ,[DatasourceLocation]
           ,[DatasourceTemplate]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[UpdatedDate]
           ,[UpdatedBy]
           ,[IsDeleted])
     VALUES
           (@Id,
			@RenderingName,
			@RenderingType,
			@Properties,
			@IsPageRendering,
			@IsRequireDatasource,
			@DatasourceLocation,
			@DatasourceTemplate,
			@CreatedDate,
			@CreatedBy,
			@UpdatedDate,
			@UpdatedBy,
			0)
	END
	ELSE
	UPDATE [Renderings] SET RenderingName = @RenderingName, IsPageRendering = @IsPageRendering,
	IsRequireDatasource = @IsRequireDatasource, DatasourceLocation = @DatasourceLocation,
	DatasourceTemplate = @DatasourceTemplate, UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy,
	RenderingType = @RenderingType
	WHERE Id = @Id
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_renderings_delete
(
	@Id uniqueidentifier
)
AS
BEGIN
	UPDATE Renderings SET IsDeleted = 1 WHERE Id = @Id
END
GO
