using RMDesktopUI.Library.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private IApiHelper _apiHelper;
        private HttpClient _apiClient;

        public SaleEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
            _apiClient = _apiHelper.ApiClient;
        }

        public async Task PostSale(SaleModel sale)
        {
            HttpResponseMessage response = await _apiClient.PostAsJsonAsync("/api/Sale", sale);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);
        }
    }
}
