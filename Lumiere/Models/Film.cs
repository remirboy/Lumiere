using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lumiere.Models
{
    public class Film
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AgeLimit { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public double Rating { get; set; }
        public TimeSpan Duration { get; set; }
        public List<FilmFeedback> Feedbacks { get; set; }
        public List<FilmPoster> Posters { get; set; }
        public List<FilmTrailer> Trailers { get; set; }
        public List<FilmSeance> Seances { get; set; }
    }
}
