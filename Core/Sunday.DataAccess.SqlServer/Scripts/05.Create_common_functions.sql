CREATE OR ALTER FUNCTION dbo.ParseIdList
(
	@IdList nvarchar(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @part NVARCHAR(255);
	DECLARE @pos INT;
	DECLARE @Result nvarchar(MAX);
	SET @Result = '';
	WHILE CHARINDEX('|', @IdList) > 0
		BEGIN
			SELECT @pos  = CHARINDEX('|', @IdList); 
			SELECT @part = SUBSTRING(@IdList, 1, @pos-1);
			SET @Result = @Result + '''' + @part + ''',';
			SET @IdList = SUBSTRING(@IdList, @pos+1, LEN(@IdList) - @pos)
		END
	 RETURN @Result + '''' + @IdList + '''';
END
GO