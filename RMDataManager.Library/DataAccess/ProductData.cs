using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;

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
    }
}
