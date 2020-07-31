CREATE PROCEDURE [dbo].[spUpdateEmployee] 
	-- Add the parameters for the stored procedure here
	@Id int, 
	@FirstName nvarchar(100),
	@LastName nvarchar(100),
	@EmailId nvarchar(100),
	@Mobile nvarchar(20),
	@Address nvarchar(MAX),
	@BirthDate nvarchar(20),
	@Employment nvarchar(20)
AS
BEGIN
	IF EXISTS (SELECT * FROM [dbo].[Employee] WHERE [Id] = @Id)
		UPDATE [dbo].[Employee] SET [FirstName] = @FirstName, [LastName] = @LastName, [EmailId] = @EmailId,
		[Mobile] = @Mobile, [Address] = @Address, [BirthDate] = @BirthDate, [Employment] = @Employment
		WHERE [Id] = @Id
END
GO