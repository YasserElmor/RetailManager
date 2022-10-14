using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel: Conductor<object>
    {
        private readonly LoginViewModel _loginVM;

        public ShellViewModel(LoginViewModel loginVM)
        {
            // here we're injecting the LoginViewModel instance through simple container
            // as we registered all ViewModels on the container earlier through reflection
            _loginVM = loginVM;
            ActivateItemAsync(_loginVM);
        }
    }
}
