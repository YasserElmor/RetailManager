﻿CREATE PROCEDURE [dbo].[spInventory_Insert]
@ProductId int,
@Quantity int,
@PurchasePrice money,
@PurchaseDate datetime2
AS
begin
	set nocount on;

	INSERT INTO dbo.Inventory(ProductId, Quantity, PurchasePrice, PurchaseDate)
	VALUES (@ProductId, @Quantity, @PurchasePrice, @PurchaseDate);

end
	