CREATE PROCEDURE [dbo].[spRegisterUser] 
	-- Add the parameters for the stored procedure here
	@Role varchar(25), 
	@EmailId varchar(25), 
	@UserName varchar(25), 
	@Password varchar(25)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[User] WHERE [UserName]=@UserName)
	BEGIN
		INSERT INTO [dbo].[User]([Role],[EmailId],[UserName],[Password])
		VALUES (@Role,@EmailId,@UserName,@Password)
	END
END
GO