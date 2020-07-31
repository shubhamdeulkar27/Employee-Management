CREATE PROCEDURE [dbo].[spAddEmployee] 
	-- Add the parameters for the stored procedure here
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@EmailId nvarchar(100),
	@Mobile nvarchar(20),
	@Address nvarchar(MAX),
	@BirthDate nvarchar(20),
	@Employment nvarchar(20)
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM [dbo].[Employee] WHERE [FirstName] = @FirstName AND [LastName] = @LastName AND [EmailId] = @EmailId)
		BEGIN
			INSERT INTO [dbo].[Employee]([FirstName],[LastName],[EmailId],[Mobile],[Address],[BirthDate],[Employment])
			VALUES (@FirstName,@LastName,@EmailId,@Mobile,@Address,@BirthDate,@Employment)
		END
END
GO
