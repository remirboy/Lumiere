using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Components
{
    public class ReservedSeatsList : ViewComponent
    {
        private readonly IFilmRepository _filmRepository;
        private readonly ISeanceRepository _seanceRepository;

        public ReservedSeatsList(IFilmRepository filmRepository, ISeanceRepository seanceRepository)
        {
            _filmRepository = filmRepository;
            _seanceRepository = seanceRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(User user)
        {
            List<ReservedFilmViewModel> reservedFilms = new List<ReservedFilmViewModel>();

            foreach(ReservedSeat reservedSeat in user.ReservedSeats)
            {
                FilmSeance seance = await _seanceRepository.GetByIdAsync(reservedSeat.SeanceId);
                if (seance == null)
                    continue;

                Film film = await _filmRepository.GetByIdAsync(seance.FilmId);
                if (film == null)
                    continue;

                ReservedFilmViewModel reservedFilm = reservedFilms.Find(f => f.SeanceId == seance.Id);
                if (reservedFilm == null)
                {
                    reservedFilm = new ReservedFilmViewModel
                    {
                        SeanceId = seance.Id,
                        FilmName = film.Name,
                        FilmPosterUrl = film.Posters.First().Url,
                        FilmDuration = film.Duration,
                        SeanceDate = seance.Date,
                        SeanceTime = seance.Time,
                        RoomNumber = seance.RoomNumber,
                        RowSeatNumbers = new Dictionary<int, int>() { { reservedSeat.RowNumber, reservedSeat.SeatsNumber } }
                    };
                }
                else
                {
                    reservedFilm.RowSeatNumbers.Add(reservedSeat.RowNumber, reservedSeat.SeatsNumber);
                }

                reservedFilms.Add(reservedFilm);
            }


            return View("ReservedSeatsList", reservedFilms);
        }
    }
}
