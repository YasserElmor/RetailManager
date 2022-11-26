using RMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<ApplicationUserModel>> GetAllAsync();
        Task<Dictionary<string, string>> GetAllRolesAsync();
        Task AddUserToRoleAsync(string userId, string roleName);
        Task RemoveUserFromRoleAsync(string userId, string roleName);
    }
}