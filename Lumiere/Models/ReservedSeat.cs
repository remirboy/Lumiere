using System;

namespace Lumiere.Models
{
    public class ReservedSeat
    {
        public Guid Id { get; set; }
        public int RowNumber { get; set; }
        public int SeatsNumber { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid SeanceId { get; set; }
        public FilmSeance Seance { get; set; }
    }
}
