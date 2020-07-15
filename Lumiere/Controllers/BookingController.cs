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
    public class BookingController : Controller
    {
        private readonly IReservedSeatRepository _reservedSeatRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISeanceRepository _seanceRepository;

        public BookingController(IReservedSeatRepository reservedSeatRepository, IFilmRepository filmRepository, IUserRepository userRepository, ISeanceRepository seanceRepository)
        {
            _reservedSeatRepository = reservedSeatRepository;
            _filmRepository = filmRepository;
            _userRepository = userRepository;
            _seanceRepository = seanceRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<Film> films = _filmRepository.GetAll().ToList();

            List<BookingFilmViewModel> bookingFilms = new List<BookingFilmViewModel>();
            foreach(Film film in films)
            {
                bookingFilms.Add(new BookingFilmViewModel {
                    FilmId = film.Id,
                    FilmName = film.Name
                });
            }

            return View(bookingFilms);
        }

        public List<ReservedSeat> GetReservedSeats()
        {
            return _reservedSeatRepository.GetAll().ToList();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ReservedSeats(ReservedSeatViewModel reservedSeats)
        {
            if (!ModelState.IsValid)
                return View("Index");

            string userId = await _userRepository.GetCurrentUserId(User);
            if (string.IsNullOrEmpty(userId))
                return View("Index");

            FilmSeance seance = new FilmSeance
            {
                Date = reservedSeats.Date,
                Time = reservedSeats.Time,
                Price = reservedSeats.Price,
                RoomNumber = reservedSeats.RoomNumber,
                FilmId = reservedSeats.FilmId
            };
            Guid seanceId = await _seanceRepository.GetIdBySeance(seance);
            if (seanceId == default)
                return View("Index");


            int seatsCountInRow = 6;
            if (reservedSeats.RoomNumber == 1)
                seatsCountInRow = 6;
            else if (reservedSeats.RoomNumber == 2)
                seatsCountInRow = 5;

            for (int i = 0; i < reservedSeats.SeatNumbers.Length; i++)
            {
                double rowNumber = reservedSeats.SeatNumbers[i] / seatsCountInRow;
                ReservedSeat reservedSeat = new ReservedSeat
                {
                    RowNumber = Convert.ToInt32(Math.Ceiling(rowNumber)),
                    SeatsNumber = reservedSeats.SeatNumbers[i],
                    UserId = userId,
                    SeanceId = seanceId
                };

                await _reservedSeatRepository.CreateAsync(reservedSeat);
            }

            return RedirectToAction("Index");
        }
    }
}