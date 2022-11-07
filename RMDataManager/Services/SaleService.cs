using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using RMDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMDataManager.Services
{
    public class SaleService
    {


        public void SaveSale(SaleModel sale, string cashierId)
        {
            List<SaleDetailDbModel> dbSaleDetails = GetSaleDetailsFromSale(sale);

            decimal subTotal = dbSaleDetails.Sum(saleDetail => saleDetail.PurchasePrice);
            decimal saleTax = dbSaleDetails.Sum(saleDetail => saleDetail.Tax);
            decimal total = saleTax + subTotal;

            SaleDbModel dbSale = new SaleDbModel()
            {
                CashierId = cashierId,
                SubTotal = subTotal,
                Tax = saleTax,
                Total = total,
            };

            SaleData saleDataAccess = new SaleData();

            saleDataAccess.SaveSaleInTransaction(dbSale, dbSaleDetails);
        }


        private List<SaleDetailDbModel> GetSaleDetailsFromSale(SaleModel sale)
        {
            ProductData productDataAccess = new ProductData();

            decimal taxRate = Library.ConfigHelper.GetTaxRate() / 100;

            List<SaleDetailDbModel> dbSaleDetails = new List<SaleDetailDbModel>();

            sale.SaleDetails.ForEach(saleDetail =>
            {
                ProductModel product = productDataAccess.GetById(saleDetail.ProductId);

                if (product == null)
                    throw new Exception("Invalid Product Id");

                decimal purchasePrice = product.RetailPrice * saleDetail.Quantity;
                decimal saleDetailTax = product.IsTaxable ? purchasePrice * taxRate : 0;

                var dbSaleDetail = new SaleDetailDbModel()
                {
                    ProductId = saleDetail.ProductId,
                    Quantity = saleDetail.Quantity,
                    Tax = saleDetailTax,
                    PurchasePrice = purchasePrice,
                };
                dbSaleDetails.Add(dbSaleDetail);
            });

            return dbSaleDetails;
        }
    }
}