using Lumiere.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public interface IPosterRepository : ICrud<FilmPoster>
    {
        IEnumerable<FilmPoster> GetByFilmId(Guid filmId);
    }
}
