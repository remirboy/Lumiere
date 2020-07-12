using Lumiere.Models;
using Lumiere.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Components
{
    public class FilmsList : ViewComponent
    {
        private readonly IFilmRepository _filmRepository;

        public FilmsList(IFilmRepository filmRepository)
        {
            _filmRepository = filmRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Film> films = _filmRepository.GetAll().ToList();

            return View("FilmsList", films);
        }
    }
}
