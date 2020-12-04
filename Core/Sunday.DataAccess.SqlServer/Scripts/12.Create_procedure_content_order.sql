CREATE OR ALTER PROCEDURE dbo.sp_contents_getOrders
(
	@ids nvarchar(MAX)
)
AS
BEGIN
	DECLARE @tblIds TABLE (id uniqueidentifier)
	INSERT INTO @tblIds SELECT value FROM string_split(@ids, '|')
	SELECT * FROM ContentOrders WHERE ContentId IN (SELECT id FROM @tblIds)
END
GO
--------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_contents_saveOrders
(
	@Orders ContentOrderType READONLY
)
AS
BEGIN
	MERGE ContentOrders AS target  
		USING (SELECT * FROM @Orders) AS source
	ON (target.ContentId = source.ContentId)  
	WHEN MATCHED THEN
		UPDATE SET target.[Order] = source.[Order]
	WHEN NOT MATCHED THEN  
		INSERT (ContentId, [Order])  
		VALUES (source.ContentId, source.[Order]);  
END
GO
