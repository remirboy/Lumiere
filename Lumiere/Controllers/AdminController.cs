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

        /// <summary>
        /// Метод подгружает страницу для редактирования ролей определенного пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя, которому нужно редактировать роли.</param>
        /// <returns>
        /// Если пользователь существует, то возвращает представление редактирования ролей, если нет - страница с
        /// ошибкой, что нужная страница не найдена.
        /// </returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            // получаем пользователя
            User user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userRepository.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRolesViewModel model = new ChangeRolesViewModel
                {
                    UserId = user.Id,
                    Name = user.FirstName,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Метод редактирует роли для определенного пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, которому нужно редактировать роли.</param>
        /// <param name="roles">Список ролей, которое нужно выставить определенному пользователю.</param>
        /// <returns>
        /// Если роли отредактированы удачно, то редирект на панель администрирования, если неудачно - страница с
        /// ошибкой, что нужная страница не найдена.
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            // Получем список ролей пользователя.
            var userRoles = await _userRepository.GetRolesAsync(user);

            // Получаем список ролей, которые были добавлены.
            var addedRoles = roles.Except(userRoles);

            // Получаем роли, которые были удалены.
            var removedRoles = userRoles.Except(roles);

            await _userRepository.AddToRolesAsync(user, addedRoles);

            await _userRepository.RemoveFromRolesAsync(user, removedRoles);

            return RedirectToAction("Index");
        }
    }
}