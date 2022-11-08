using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using RMDataManager.Models;
using RMDataManager.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RMDataManager.Controllers
{

    [Authorize]
    public class SaleController : ApiController
    {
        public void Post(SaleModel sale)
        {
            string cashierId = RequestContext.Principal.Identity.GetUserId();

            SaleService saleService = new SaleService();

            saleService.SaveSale(sale, cashierId);
        }

        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData saleDataAccess = new SaleData();

            var salesReport = saleDataAccess.GetSaleReport();

            return salesReport;
        }
    }
}
