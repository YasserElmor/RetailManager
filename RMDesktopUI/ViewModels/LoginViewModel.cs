using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;
using System;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IApiHelper _apiHelper;
        private readonly IEventAggregator _events;

        public LoginViewModel(IApiHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        //TODO: Remove embedded values before pushing to production
        private string _username = "elmor.yasser@gmail.com";
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

        private string _password = "DummyPassword0#";
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

                await _apiHelper.GetLoggedInUserModel(result.Access_Token);

                await _events.PublishOnUIThreadAsync(new LogOnEvent());
            }
            catch (Exception)
            {
                ErrorMessage = "Invalid Username or Password";
            }
        }


    }
}
