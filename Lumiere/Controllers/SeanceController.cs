using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lumiere.Controllers
{
    public class SeanceController : Controller
    {
        private readonly IFilmRepository _filmRepository;
        private readonly ISeanceRepository _seanceRepository;

        public SeanceController(IFilmRepository filmRepository, ISeanceRepository seanceRepository)
        {
            _filmRepository = filmRepository;
            _seanceRepository = seanceRepository;
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

            FilmSeance seance = new FilmSeance
            {
                Date = model.Date,
                Time = model.Time,
                Price = model.Price,
                FilmId = model.FilmId
            };

            await _seanceRepository.CreateAsync(seance);

            film.Seances.Add(seance);
            await _filmRepository.UpdateAsync(film);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            FilmSeance seance = await _seanceRepository.GetByIdAsync(id);
            if (seance == null)
                return NotFound();

            await _seanceRepository.DeleteAsync(seance);

            return RedirectToAction("Index", "Admin");
        }
    }
}