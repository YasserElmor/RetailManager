using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.Models;
using System.Threading;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _salesVM;
        private readonly SimpleContainer _container;
        private readonly ILoggedInUserModel _user;
        private readonly IEventAggregator _events;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM,
            SimpleContainer container, ILoggedInUserModel user)
        {
            _events = events;
            _salesVM = salesVM;
            _container = container;
            _user = user;

            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async Task ExitApplicationAsync()
        {
            await TryCloseAsync();
        }

        public async Task UserManagementAsync()
        {
            await ActivateItemAsync(_container.GetInstance<UserDisplayViewModel>());
        }

        public bool IsLoggedIn => string.IsNullOrWhiteSpace(_user.Token) == false;

        public async Task LogOutAsync()
        {
            _user.LogOffUser();
            NotifyOfPropertyChange(() => IsLoggedIn);
            await ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
