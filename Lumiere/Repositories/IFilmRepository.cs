using Lumiere.Models;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public interface IFilmRepository : ICrud<Film>
    {
        Task<Film> GetByNameAsync(string name);
    }
}
