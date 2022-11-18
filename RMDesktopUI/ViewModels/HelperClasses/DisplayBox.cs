using Caliburn.Micro;
using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;

namespace RMDesktopUI.ViewModels.HelperClasses
{
    public class DisplayBox : IDisplayBox
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        public DisplayBox(StatusInfoViewModel status, IWindowManager window)
        {
            _status = status;
            _window = window;
        }

        public async Task DisplayUnauthorizedMessageBoxAsync(Exception ex)
        {
            string caughtExceptionMessage = "Unauthorized";
            string displayHeader = "Unauthorized Access";
            string displayMessage = "You do not have permission to interact with this Page";

            await DisplayGenericMessageBoxAsync(
                    caughtExceptionMessage,
                    displayHeader,
                    displayMessage,
                    ex);
        }

        private async Task DisplayGenericMessageBoxAsync(string exceptionMessage, string header, string displayMessage,
            Exception ex)
        {
            if (ex.Message == exceptionMessage)
                await Configure(header, displayMessage);
            else
                await Configure("Fatal Exception", ex.Message);
        }

        private async Task Configure(string header, string message)
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.Title = "System Error";

            _status.UpdateMessage(header, message);
            await _window.ShowDialogAsync(_status, null, settings);
        }

    }
}
