using Caliburn.Micro;
using RMDesktopUI.Helpers;
using RMDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IApiHelper _apiHelper;

        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }


        public bool IsErrorVisible => ErrorMessage?.Length > 0;

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }


        public bool CanLogIn
        {
            get
            {
                bool output = false;

                if (Username?.Length > 0 && Password?.Length > 0)
                    output = true;

                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                AuthenticatedUser result = await _apiHelper.Authenticate(Username, Password);

                // in its current state, the ApiHelper's Authenticate method would return a response of type AuthenticatedUser if the input data is valid
            }
            catch (Exception)
            {
                ErrorMessage = "Invalid Username or Password";
            }
        }


    }
}
