using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lumiere.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;


        public AdminController(RoleManager<IdentityRole> roleManager, IUserRepository userRepository)
        {
            _roleManager = roleManager;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            List<User> users = _userRepository.GetAll().ToList();
            return View(users);
        }
    }
}