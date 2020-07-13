using Lumiere.Data;
using Lumiere.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public class TrailerRepository : ITrailerRepository
    {
        private readonly LumiereContext _context;

        public TrailerRepository(LumiereContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(FilmTrailer trailer)
        {
            await SaveStateAsync(trailer, EntityState.Added);
        }

        public async Task DeleteAsync(FilmTrailer trailer)
        {
            _context.Remove(trailer);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<FilmTrailer> GetAll()
        {
            return _context.Trailers;
        }

        public IEnumerable<FilmTrailer> GetByFilmId(Guid filmId)
        {
            return _context.Trailers.Where(w => w.FilmId == filmId);
        }

        public async Task<FilmTrailer> GetByIdAsync(Guid id)
        {
            return await _context.Trailers.SingleOrDefaultAsync(sod => sod.Id == id);
        }

        public async Task UpdateAsync(FilmTrailer trailer)
        {
            await SaveStateAsync(trailer, EntityState.Modified);
        }

        private async Task SaveStateAsync(FilmTrailer trailer, EntityState state)
        {
            _context.Entry(trailer).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
