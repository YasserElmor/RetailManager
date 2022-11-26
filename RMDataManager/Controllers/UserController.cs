using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using RMDataManager.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public async Task<List<ApplicationUserModel>> GetAllUsersAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = await userManager.Users.ToListAsync();
                var roles = await context.Roles.ToListAsync();


                List<ApplicationUserModel> applicationUsers = users
                    .Select(user =>
                {
                    var applicationUser = new ApplicationUserModel()
                    {
                        Id = user.Id,
                        Email = user.Email,
                    };

                    user.Roles.ForEach(role =>
                    {
                        applicationUser.Roles.Add(role.RoleId, roles.Where(r => r.Id == role.RoleId).First().Name);
                    });

                    return applicationUser;
                })
                    .ToList();

                return applicationUsers;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public async Task<Dictionary<string, string>> GetAllRolesAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var roles = await context.Roles.ToDictionaryAsync(role => role.Id, role => role.Name);

                return roles;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public async Task AddRoleAsync(UserRolePairModel userRole)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                await userManager.AddToRoleAsync(userRole.UserId, userRole.RoleName);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public async Task RemoveRoleAsync(UserRolePairModel userRole)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                await userManager.RemoveFromRoleAsync(userRole.UserId, userRole.RoleName);
            }
        }
    }
}
