using Lumiere.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public interface IFilmRepository
    {
        Task CreateAsync(Film film);
        Task UpdateAsync(Film film);
        Task DeleteAsync(Film film);
        IEnumerable<Film> GetAll();
        Task<Film> GetByIdAsync(Guid id);
        Task<Film> GetByNameAsync(string name);
    }
}
