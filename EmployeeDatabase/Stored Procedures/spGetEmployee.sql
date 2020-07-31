CREATE PROCEDURE [dbo].[spGetEmployee] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT * FROM [dbo].[Employee] WHERE Id = @Id
	
END
GO