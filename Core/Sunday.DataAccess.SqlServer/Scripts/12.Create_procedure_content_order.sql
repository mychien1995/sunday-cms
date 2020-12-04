CREATE OR ALTER PROCEDURE dbo.sp_contents_getOrders
(
	@ids nvarchar(MAX)
)
AS
BEGIN
	DECLARE @tblIds TABLE (id uniqueidentifier)
	INSERT INTO @tblIds SELECT value FROM string_split(@ids, '|')
	SELECT Id AS ContentId, SortOrder AS [Order] FROM Contents WHERE Id IN (SELECT id FROM @tblIds)
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_contents_saveOrders
(
	@Orders ContentOrderType READONLY
)
AS
BEGIN
	MERGE Contents AS target  
		USING (SELECT * FROM @Orders) AS source
	ON (target.Id = source.ContentId)  
	WHEN MATCHED THEN
		UPDATE SET target.[SortOrder] = source.[Order];
END
GO