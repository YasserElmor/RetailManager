using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSaleInTransaction(SaleDbModel sale, List<SaleDetailDbModel> saleDetails)
        {
            SqlDataAccess sql = new SqlDataAccess();

            try
            {
                sql.StartTransaction("RMData");
                sql.SaveDataInTransaction("dbo.spSaleInsert", sale);

                var p = new { sale.CashierId, sale.SaleDate };
                sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSaleLookup", p).SingleOrDefault();

                saleDetails.ForEach(detail =>
                {
                    detail.SaleId = sale.Id;
                    sql.SaveDataInTransaction("dbo.spSaleDetailInsert", detail);
                });

                sql.CommitTransaction();
            }
            catch
            {
                sql.RollbackTransaction();
                throw;
            }
        }
    }
}
