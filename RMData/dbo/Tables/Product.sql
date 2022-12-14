CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ProductName] NVARCHAR(100) NOT NULL, 
	[Description] NVARCHAR(MAX) NOT NULL, 
	[RetailPrice] MONEY NOT NULL, /* Default Selling Price */
	[QuantityInStock] int NOT NULL DEFAULT 1, /* Inventory Quantity minus sold units in the SaleDetail aggregate */
	[IsTaxable] bit NOT NULL DEFAULT 1,
	[CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
	[LastModified] DATETIME2 NOT NULL DEFAULT getutcdate()
)
