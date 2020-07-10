using Lumiere.Models;
using Lumiere.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lumiere.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly LumiereContext _context;

        public FilmRepository(LumiereContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Film film)
        {
            await SaveState(film, EntityState.Added);
        }

        public async Task UpdateAsync(Film film)
        {
            await SaveState(film, EntityState.Modified);
        }

        public async Task DeleteAsync(Film film)
        {
            _context.Remove(film);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Film> GetAll()
        {
            return _context.Films;
        }

        public async Task<Film> GetByIdAsync(Guid id)
        {
            return await _context.Films
                .Include(i => i.Posters)
                .Include(i => i.Trailers)
                .Include(i => i.Seances)
                .Include(i => i.Feedbacks)
                .SingleOrDefaultAsync(sod => sod.Id == id);
        }

        public async Task<Film> GetByNameAsync(string name)
        {
            return await _context.Films
                .Include(i => i.Posters)
                .Include(i => i.Trailers)
                .Include(i => i.Seances)
                .Include(i => i.Feedbacks)
                .SingleOrDefaultAsync(sod => sod.Name == name);
        }

        private async Task SaveState(Film film, EntityState state)
        {
            _context.Entry(film).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
