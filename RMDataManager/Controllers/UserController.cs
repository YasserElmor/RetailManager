using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        // GET api/User
        public UserModel GetById()
        {
            // TODO: Disconnect this dependency using DI
            UserData data = new UserData();

            string userId = RequestContext.Principal.Identity.GetUserId();

            UserModel userData = data.GetUserById(userId).First();

            return userData;
        }
    }
}
