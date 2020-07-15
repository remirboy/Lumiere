using Lumiere.Models;
using System;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public interface ISeanceRepository : ICrud<FilmSeance>
    {
        Task<Guid> GetIdBySeance(FilmSeance seance);
    }
}
