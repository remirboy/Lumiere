using Lumiere.Models;
using System;
using System.Collections.Generic;

namespace Lumiere.Repositories
{
    interface ITrailerRepository : ICrud<FilmTrailer>
    {
        IEnumerable<FilmTrailer> GetByFilmId(Guid filmId);
    }
}
