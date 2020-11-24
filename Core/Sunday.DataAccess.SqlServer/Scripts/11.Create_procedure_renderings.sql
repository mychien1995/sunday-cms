CREATE OR ALTER PROCEDURE dbo.sp_renderings_search
(
	@Text nvarchar(1000),
	@PageSize int,
	@PageIndex int
)
AS
BEGIN
	IF @PageSize IS NULL
		SET @PageSize = 10
	IF @PageIndex IS NULL
		SET @PageIndex = 0

	SELECT COUNT(*) FROM Renderings WHERE IsDeleted = 0 AND
	(@Text IS NULL OR LEN(TRIM(@Text)) = 0 OR RenderingName LIKE '%' + @Text +'%')

	SELECT * FROM Renderings WHERE IsDeleted = 0 AND
	(@Text IS NULL OR LEN(TRIM(@Text)) = 0 OR RenderingName LIKE '%' + @Text +'%')
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
	DatasourceTemplate = @DatasourceTemplate, UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy
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
