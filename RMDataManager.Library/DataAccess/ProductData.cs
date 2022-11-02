using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace RMDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            // TODO: set up AutoFac for DI, and code against interfaces instead of concrete classes
            SqlDataAccess sql = new SqlDataAccess();

            List<ProductModel> data = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetAll", new { }, "RMData");

            return data;
        }

        public ProductModel GetById(int Id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var parameters = new { Id };

            List<ProductModel> data = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetById", parameters, "RMData");

            ProductModel product = data.SingleOrDefault(p => p.Id == Id);

            return product;
        }

    }
}
