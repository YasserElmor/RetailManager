using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.Api
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiClient;
        private readonly ILoggedInUserModel _loggedInUser;
        public ApiHelper(ILoggedInUserModel loggedInUserModel)
        {
            _loggedInUser = loggedInUserModel;
            InitializeClient();
        }

        public HttpClient ApiClient => _apiClient;

        private void InitializeClient()
        {
            _apiClient = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["api"])
            };
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string,string>("password",password)
            });

            // we're only passing /Token as we've already established the HttpClient's BaseAddress during initialization
            using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                // to access the ReadAsAsync<> Ext. Method, we'll need to install the following package: Microsoft.AspNet.WebApi.Client to RMDesktopUI.Library's References
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    return result;
                }

                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task GetLoggedInUserModel(string token)
        {
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    // now instead of returning the result, we'll pass it into a singleton 
                    // for it to be accessed throughout the application
                    // for that we'll need to extract an ILoggedInUserModel interface
                    // and register it as a singleton at in the Bootstrapper's DI SimpleContainer
                    // and lastly inject it to the consturctor of this class

                    _loggedInUser.Token = token;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.EmailAddress = result.EmailAddress;

                }

                else
                {
                    throw new Exception(response.ReasonPhrase);
                }

            }
        }

    }
}
