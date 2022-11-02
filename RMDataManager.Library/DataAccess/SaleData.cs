using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System.Linq;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleDbModel sale)
        {
            SqlDataAccess sql = new SqlDataAccess();

            sql.SaveData("dbo.spSaleInsert", sale, "RMData");
        }

        public void SaveSaleDetail(SaleDetailDbModel saleDetail)
        {
            SqlDataAccess sql = new SqlDataAccess();

            sql.SaveData("dbo.spSaleDetailInsert", saleDetail, "RMData");
        }

        public int GetSaleId(SaleDbModel sale)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { sale.CashierId, sale.SaleDate };

            int id = sql.LoadData<int, dynamic>("dbo.spSaleLookup", p, "RMData").SingleOrDefault();

            return id;
        }
    }
}
