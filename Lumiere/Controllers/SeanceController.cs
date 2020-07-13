using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lumiere.Controllers
{
    public class SeanceController : Controller
    {
        private readonly IFilmRepository _filmRepository;

        public SeanceController(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateSeanceViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            Film film = await _filmRepository.GetByIdAsync(model.FilmId);
            if(film == null)
            {
                ModelState.AddModelError(string.Empty, "Фильм не найден.");
                return PartialView(model);
            }
                

            return PartialView();
        }
    }
}