using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Lumiere.ViewModels
{
    public class ChangeRolesViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }

        public ChangeRolesViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
