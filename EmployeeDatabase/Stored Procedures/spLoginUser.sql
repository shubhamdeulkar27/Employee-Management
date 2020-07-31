CREATE PROCEDURE [dbo].[spLoginUser] 
	-- Add the parameters for the stored procedure here
	@UserName nvarchar(25), 
	@Password nvarchar(25)
AS
    SET NOCOUNT ON
BEGIN
	SELECT * FROM [dbo].[User] WHERE [UserName]=@UserName AND [Password]=@Password
END
GO