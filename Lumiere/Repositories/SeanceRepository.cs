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

        public async Task<Guid> GetIdBySeance(FilmSeance seance)
        {
            FilmSeance filmSeance = await _context.Seances.SingleOrDefaultAsync(sod =>
               sod.Date == seance.Date &&
               sod.Time == seance.Time &&
               sod.Price == seance.Price &&
               sod.RoomNumber == seance.RoomNumber &&
               sod.FilmId == seance.FilmId);

            return filmSeance.Id;
        }

        public IEnumerable<FilmSeance> GetByFilmId(Guid filmId)
        {
            return _context.Seances.Where(w => w.FilmId == filmId);
        }

        private async Task SaveState(FilmSeance seance, EntityState state)
        {
            _context.Entry(seance).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
