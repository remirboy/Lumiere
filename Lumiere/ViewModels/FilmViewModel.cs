using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.ViewModels
{
    public class FilmViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AgeLimit { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string TrailerUrl { get; set; }
    }
}
