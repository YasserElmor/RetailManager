using Microsoft.AspNet.Identity;
using RMDataManager.Models;
using RMDataManager.Services;
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
    }
}
