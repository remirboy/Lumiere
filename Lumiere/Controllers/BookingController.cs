﻿using System;
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
                double rowNumber = (reservedSeats.SeatNumbers[i] + 1) / seatsCountInRow;
                ReservedSeat reservedSeat = new ReservedSeat
                {
                    RowNumber = Convert.ToInt32(Math.Ceiling(rowNumber)),
                    SeatsNumber = reservedSeats.SeatNumbers[i] + 1,
                    UserId = userId,
                    SeanceId = seanceId
                };

                await _reservedSeatRepository.CreateAsync(reservedSeat);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DatesList(Guid filmId)
        {
            List<FilmSeance> seances = _seanceRepository.GetByFilmId(filmId).ToList();

            List<DateTime> dates = new List<DateTime>();
            foreach (FilmSeance seance in seances)
                dates.Add(seance.Date);

            return PartialView(dates);
        }

        [HttpGet]
        public IActionResult TimesList(Guid filmId, string date)
        {
            List<FilmSeance> seances = _seanceRepository.GetByFilmId(filmId).ToList();

            List<DateTime> times = new List<DateTime>();
            foreach (FilmSeance seance in seances)
            {
                if (seance.Date == DateTime.Parse(date))
                    times.Add(seance.Time);
            }

            return PartialView(times);
        }

        [HttpGet]
        public IActionResult RoomNumbersList(Guid filmId, string date, string time)
        {
            if (filmId == default)
                return PartialView(new List<int>());

            List<FilmSeance> seances = _seanceRepository.GetByFilmId(filmId).ToList();
            if (seances == null)
                return PartialView(new List<int>());

            if (!DateTime.TryParse(date, out DateTime seanceDate))
                return PartialView(new List<int>());

            if (!DateTime.TryParse(time, out DateTime seanceTime))
                return PartialView(new List<int>());

            List<int> roomNumbers = new List<int>();
            foreach (FilmSeance seance in seances)
            {
                if (seance.Date == seanceDate && seance.Time == seanceTime)
                    roomNumbers.Add(seance.RoomNumber);
            }

            return PartialView(roomNumbers);
        }
    }
}