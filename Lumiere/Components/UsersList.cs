using Lumiere.Models;
using Lumiere.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Components
{
    public class UsersList : ViewComponent
    {
        private readonly IUserRepository _userRepository;

        public UsersList(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<User> users = _userRepository.GetAll().ToList();

            return View("UsersList", users);
        }
    }
}
