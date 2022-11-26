using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;
using RMDesktopUI.ViewModels.HelperClasses;
using System;
using System.ComponentModel;
using System.Linq;
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

        private ApplicationUserModel _selectedUser;
        public ApplicationUserModel SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;

                UserRoles = new BindingList<string>(value.Roles.Values.ToList());
                LoadAvailableRoles();

                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        private string _selectedUserName;
        public string SelectedUserName
        {
            get
            {
                return _selectedUserName;
            }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        private BindingList<string> _userRoles = new BindingList<string>();
        public BindingList<string> UserRoles
        {
            get
            {
                return _userRoles;
            }
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        private BindingList<string> _availableRoles = new BindingList<string>();
        public BindingList<string> AvailableRoles
        {
            get
            {
                return _availableRoles;
            }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        private string _selectedUserRole;
        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set
            {
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }

        private string _selectedAvailableRole;
        public string SelectedAvailableRole
        {
            get { return _selectedAvailableRole; }
            set
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }


        private async Task LoadAvailableRoles()
        {
            var roles = await _userEndpoint.GetAllRolesAsync();

            foreach (var role in roles)
            {
                if (!UserRoles.Contains(role.Value))
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        public async void AddSelectedRole()
        {
            await _userEndpoint.AddUserToRoleAsync(SelectedUser.Id, SelectedAvailableRole);
            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);
        }

        public async void RemoveSelectedRole()
        {
            await _userEndpoint.RemoveUserFromRoleAsync(SelectedUser.Id, SelectedUserRole);
            AvailableRoles.Add(SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);
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
