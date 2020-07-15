using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lumiere.Models;
using Lumiere.Repositories;
using Lumiere.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lumiere.Controllers
{
    public class BookingController : Controller
    {
        private readonly IReservedSeatRepository _reservedSeatRepository;

        public BookingController(IReservedSeatRepository reservedSeatRepository)
        {
            _reservedSeatRepository = reservedSeatRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<ReservedSeat> GetReservedSeats()
        {
            return _reservedSeatRepository.GetAll().ToList();
        }

        [HttpPost]
        public IActionResult ReservedSeats(ReservedSeatViewModel reservedSeats)
        {



            return RedirectToAction("Index");
        }
    }
}