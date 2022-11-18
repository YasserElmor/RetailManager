using System;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels.HelperClasses
{
    public interface IDisplayBox
    {
        Task DisplayUnauthorizedMessageBoxAsync(Exception ex);
    }
}