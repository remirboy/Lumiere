using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lumiere.Controllers
{
    public class FilmController : Controller
    {
        private readonly IFilmRepository _filmRepository;
        private readonly ITrailerRepository _trailerRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IPosterRepository _posterRepository;

        public FilmController(IFilmRepository filmRepository, ITrailerRepository trailerRepository, IWebHostEnvironment appEnvironment, IPosterRepository posterRepository)
        {
            _filmRepository = filmRepository;
            _trailerRepository = trailerRepository;
            _appEnvironment = appEnvironment;
            _posterRepository = posterRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            Film film = await _filmRepository.GetByIdAsync(id);
            if (film == null)
                return NotFound();

            return View(film);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(FilmViewModel model, IFormFileCollection posters)
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

            FilmTrailer trailer = new FilmTrailer
            {
                Url = model.TrailerUrl,
                FilmId = film.Id
            };
            await _trailerRepository.CreateAsync(trailer);
            film.Trailers.Add(trailer);

            List<FilmPoster> filmPosters = await SavePosters(film.Id, posters);
            film.Posters.AddRange(filmPosters);

            await _filmRepository.UpdateAsync(film);

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(Guid id)
        {
            Film film = await _filmRepository.GetByIdAsync(id);
            if (film == null)
                return NotFound();

            FilmViewModel filmViewModel = new FilmViewModel
            {
                Id = film.Id,
                Name = film.Name,
                Description = film.Description,
                AgeLimit = film.AgeLimit,
                ReleaseDate = film.ReleaseDate,
                Duration = film.Duration
            };

            return View(filmViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(FilmViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Film film = new Film
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                AgeLimit = model.AgeLimit,
                ReleaseDate = model.ReleaseDate,
                Duration = model.Duration
            };

            await _filmRepository.UpdateAsync(film);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Film film = await _filmRepository.GetByIdAsync(id);
            if (film == null)
                return NotFound();

            await _filmRepository.DeleteAsync(film);

            return RedirectToAction("Index", "Admin");
        }

        private async Task<List<FilmPoster>> SavePosters(Guid filmId, IFormFileCollection posters)
        {
            List<FilmPoster> filmPosters = new List<FilmPoster>();
            Guid posterId = Guid.NewGuid();
            foreach (var image in posters)
            {
                using (var fileStream = new FileStream($"{_appEnvironment.WebRootPath}/img/posters/poster_{posterId}.png", FileMode.Create, FileAccess.Write))
                {
                    image.CopyTo(fileStream);

                    FilmPoster filmPoster = new FilmPoster
                    {
                        Url = $"/img/posters/poster_{posterId}.png",
                        FilmId = filmId
                    };
                    await _posterRepository.CreateAsync(filmPoster);

                    filmPosters.Add(filmPoster);
                }
                posterId = Guid.NewGuid();
            }

            return filmPosters;
        }
    }
}