CREATE PROCEDURE [dbo].[spSaleDetail_Insert]
	@Id int output,
	@SaleId int,
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@Tax money

AS
begin
	set nocount on;
	INSERT INTO dbo.SaleDetail(SaleId, ProductId, Quantity, PurchasePrice, Tax)
	VALUES (@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax);

	SELECT @Id = @@IDENTITY;
end