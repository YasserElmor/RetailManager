using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Api
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IApiHelper _apiHelper;
        private readonly HttpClient _apiClient;

        public UserEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
            _apiClient = _apiHelper.ApiClient;
        }

        public async Task<List<ApplicationUserModel>> GetAllAsync()
        {
            HttpResponseMessage response = await _apiClient.GetAsync("api/User/Admin/GetAllUsers");

            if (response.IsSuccessStatusCode)
            {
                List<ApplicationUserModel> result = await response.Content.ReadAsAsync<List<ApplicationUserModel>>();
                return result;
            }

            else
                throw new Exception(response.ReasonPhrase);
        }

        public async Task<Dictionary<string, string>> GetAllRolesAsync()
        {
            HttpResponseMessage response = await _apiClient.GetAsync("api/User/Admin/GetAllRoles");

            if (response.IsSuccessStatusCode)
            {
                Dictionary<string, string> result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                return result;
            }

            else
                throw new Exception(response.ReasonPhrase);
        }

        public async Task AddUserToRoleAsync(string userId, string roleName)
        {
            var data = new { userId, roleName };

            HttpResponseMessage response = await _apiClient.PostAsJsonAsync("api/User/Admin/AddRole", data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var data = new { userId, roleName };

            HttpResponseMessage response = await _apiClient.PostAsJsonAsync("api/User/Admin/RemoveRole", data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }

        }
    }
}
