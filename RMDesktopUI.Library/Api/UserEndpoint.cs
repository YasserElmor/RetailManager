using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Api
{
    public class UserEndpoint : IUserEndpoint
    {
        private IApiHelper _apiHelper;
        private HttpClient _apiClient;

        public UserEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
            _apiClient = _apiHelper.ApiClient;
        }

        public async Task<List<ApplicationUserModel>> GetAllAsync()
        {
            HttpResponseMessage response = await _apiClient.GetAsync("Admin/GetAllUsers");

            if (response.IsSuccessStatusCode)
            {
                List<ApplicationUserModel> result = await response.Content.ReadAsAsync<List<ApplicationUserModel>>();
                return result;
            }

            else
                throw new Exception(response.ReasonPhrase);
        }
    }
}
