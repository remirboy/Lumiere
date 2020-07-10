using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumiere.Controllers
{
    public class FilmController : Controller
    {
        private readonly IFilmRepository _filmRepository;

        public FilmController(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilmViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Film film = new Film
            {
                Name = model.Name,
                Description = model.Description,
                AgeLimit = model.AgeLimit,
                ReleaseDate = model.ReleaseDate,
                Duration = model.Duration
            };

            await _filmRepository.CreateAsync(film);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            Film film = await _filmRepository.GetByIdAsync(id);
            if (film == null)
                return NotFound();

            await _filmRepository.DeleteAsync(film);

            return RedirectToAction("Index", "Home");
        }
    }
}