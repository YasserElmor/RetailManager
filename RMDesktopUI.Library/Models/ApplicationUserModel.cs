using System.Collections.Generic;

namespace RMDesktopUI.Library.Models
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        // the key is the role's Id while the value is the role's Name
        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();

        // display property specific to the View
        public string RoleList => string.Join(", ", Roles.Values);
    }
}
