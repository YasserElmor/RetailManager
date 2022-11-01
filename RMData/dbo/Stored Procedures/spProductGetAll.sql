CREATE PROCEDURE [dbo].[spProductGetAll]
AS
begin
	set nocount on;
	SELECT Id, ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable
	FROM dbo.Product
	ORDER BY ProductName;
end