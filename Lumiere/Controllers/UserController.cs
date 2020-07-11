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

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string id)
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

            EditProfileViewModel editProfile = new EditProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                DateOfBirth = user.DateOfBirth
            };

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

            IdentityResult result =  await _userRepository.UpdateAsync(user);
            if (result.Succeeded)
                return View("Index", user);

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

            return RedirectToAction("Index", id);
        }
    }
}