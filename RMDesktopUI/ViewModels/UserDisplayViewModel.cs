using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;
using RMDesktopUI.ViewModels.HelperClasses;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly IDisplayBox _displayBox;
        private BindingList<ApplicationUserModel> _users;

        public BindingList<ApplicationUserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserDisplayViewModel(IUserEndpoint userEndpoint, IDisplayBox displayBox)
        {
            _userEndpoint = userEndpoint;
            _displayBox = displayBox;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                var users = await _userEndpoint.GetAllAsync();

                Users = new BindingList<ApplicationUserModel>(users);
            }
            catch (Exception ex)
            {
                await _displayBox.DisplayUnauthorizedMessageBoxAsync(ex);

                await TryCloseAsync();
            }
        }
    }
}
