CREATE PROCEDURE [dbo].[spUser_Lookup]
	@Id nvarchar(128)
AS
begin
	set nocount on;

	SELECT Id, FirstName, LastName, EmailAddress, CreatedDate 
	FROM [dbo].[User]
	WHERE Id = @Id;
end