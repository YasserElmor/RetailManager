CREATE PROCEDURE [dbo].[spSaleLookup]
	@CashierId nvarchar(128),
	@SaleDate datetime2
AS
begin
	set nocount on;

	SELECT Id
	FROM [dbo].[Sale]
	WHERE CashierId = @CashierId and SaleDate = @SaleDate;
end