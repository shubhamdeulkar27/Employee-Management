CREATE PROCEDURE [dbo].[spDeleteEmployee] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	DELETE FROM Employee WHERE Id = @Id
END
GO