using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Api
{
    public class ProductEndpoint : IProductEndpoint
    {
        private IApiHelper _apiHelper;
        private HttpClient _apiClient;

        public ProductEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
            _apiClient = _apiHelper.ApiClient;
        }

        public async Task<List<ProductModel>> GetAllAsync()
        {
            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<ProductModel> result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }

                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
