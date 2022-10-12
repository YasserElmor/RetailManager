CREATE TABLE [dbo].[SaleDetail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SaleId] INT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [Quantity] INT NOT NULL DEFAULT 1,
    [PurchasePrice] MONEY NOT NULL, /* this is used alongside the default product selling RetailPrice in case we wanted to sell it for a different price (discounts, exceptions, etc.) */
    [Tax] MONEY NOT NULL DEFAULT 0,
)
