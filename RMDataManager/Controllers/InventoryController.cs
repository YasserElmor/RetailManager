using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        [Authorize(Roles = "Admin,Manager")]
        public List<InventoryModel> Get()
        {
            InventoryData inventoryDataAccess = new InventoryData();

            var inventory = inventoryDataAccess.GetInventory();

            return inventory;
        }


        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {
            InventoryData inventoryDataAccess = new InventoryData();

            inventoryDataAccess.SaveInventoryRecord(item);
        }


    }
}
