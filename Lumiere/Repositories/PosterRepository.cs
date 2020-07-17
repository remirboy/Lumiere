using Lumiere.Data;
using Lumiere.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public class PosterRepository : IPosterRepository
    {
        private readonly LumiereContext _context;

        public PosterRepository(LumiereContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(FilmPoster poster)
        {
            await SaveStateAsync(poster, EntityState.Added);
        }

        public async Task DeleteAsync(FilmPoster poster)
        {
            _context.Remove(poster);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<FilmPoster> GetAll()
        {
            return _context.Posters;
        }

        public IEnumerable<FilmPoster> GetByFilmId(Guid filmId)
        {
            return _context.Posters.Where(w => w.FilmId == filmId);
        }

        public async Task<FilmPoster> GetByIdAsync(Guid id)
        {
            return await _context.Posters.SingleOrDefaultAsync(sod => sod.Id == id);
        }

        public async Task UpdateAsync(FilmPoster poster)
        {
            await SaveStateAsync(poster, EntityState.Modified);
        }

        private async Task SaveStateAsync(FilmPoster poster, EntityState state)
        {
            _context.Entry(poster).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
