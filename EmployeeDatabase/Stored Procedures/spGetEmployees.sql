CREATE PROCEDURE [dbo].[spGetEmployees]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT * FROM [dbo].[Employee] ORDER BY Id ASC
END
GO