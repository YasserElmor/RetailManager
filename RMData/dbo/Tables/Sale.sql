CREATE TABLE [dbo].[Sale]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[CashierId] NVARCHAR(128) NOT NULL, /* Reference to the User Id which references the Auth Id */ 
	[SaleDate] DATETIME2(7) NOT NULL, 
	[SubTotal] MONEY NOT NULL, 
	[Tax] MONEY NOT NULL, /* this is the overall tax value for the sale, but different items have different taxable values that changes over time */
						  /* for that we'll add a Tax record specific to each item in the SaleDetail table */
	[Total] MONEY NOT NULL, 
	CONSTRAINT [FK_Sale_ToUser] FOREIGN KEY (CashierId) REFERENCES [User](Id),

)
