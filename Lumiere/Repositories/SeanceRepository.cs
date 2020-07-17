using Lumiere.Data;
using Lumiere.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public class SeanceRepository : ISeanceRepository
    {
        private readonly LumiereContext _context;

        public SeanceRepository(LumiereContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(FilmSeance seance)
        {
            await SaveState(seance, EntityState.Added);
        }

        public async Task UpdateAsync(FilmSeance seance)
        {
            await SaveState(seance, EntityState.Modified);
        }

        public async Task DeleteAsync(FilmSeance seance)
        {
            _context.Remove(seance);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<FilmSeance> GetAll()
        {
            return _context.Seances.Include(i => i.ReservedSeats);
        }

        public async Task<FilmSeance> GetByIdAsync(Guid id)
        {
            return await _context.Seances
                .Include(i => i.ReservedSeats)
                .SingleOrDefaultAsync(sod => sod.Id == id);
        }

        private async Task SaveState(FilmSeance seance, EntityState state)
        {
            _context.Entry(seance).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
