using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lumiere.Models
{
    public class FilmSeance
    {
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }
        public int Price { get; set; }
        public Guid FilmId { get; set; }
        public Film Film { get; set; }
        public List<ReservedSeat> ReservedSeats { get; set; }

        public FilmSeance()
        {
            ReservedSeats = new List<ReservedSeat>();
        }
    }
}
