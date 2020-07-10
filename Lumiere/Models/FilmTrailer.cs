using System;

namespace Lumiere.Models
{
    public class FilmTrailer
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Guid FilmId { get; set; }
        public Film Film { get; set; }
    }
}
