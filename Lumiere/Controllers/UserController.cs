using System.Threading.Tasks;
using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lumiere.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUserRepository userRepository, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            bool userIsAdmin = await _userRepository.IsInRoleAsync(user, "admin");

            EditProfileViewModel editProfile = new EditProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                DateOfBirth = user.DateOfBirth,
                IsAdmin = userIsAdmin
            };

            string currentUserId = await _userRepository.GetCurrentUserId(User);
            User currentUser = await _userRepository.GetByIdAsync(currentUserId);
            if (await _userRepository.IsInRoleAsync(currentUser, "admin"))
                ViewBag.CurrentUserIsAdmin = true;
            else
                ViewBag.CurrentUserIsAdmin = false;

            return View(editProfile);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user = await _userRepository.GetByIdAsync(model.Id);

            user.FirstName = model.FirstName;
            user.SecondName = model.SecondName;
            user.DateOfBirth = model.DateOfBirth;

            if (model.IsAdmin)
            {
                await _userRepository.AddToRoleAsync(user, "admin");
            }
            else
            {
                if (await _userRepository.IsInRoleAsync(user, "admin"))
                    await _userRepository.RemoveFromRoleAsync(user, "admin");
            }

            IdentityResult result =  await _userRepository.UpdateAsync(user);
            if (result.Succeeded)
                return View("Profile", user.Id);

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userRepository.DeleteAsync(user);

            return RedirectToAction("Index", "Admin");
        }
    }
}