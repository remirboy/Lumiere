using Lumiere.Data;
using Lumiere.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public class ReservedSeatRepository : IReservedSeatRepository
    {
        private readonly LumiereContext _context;

        public ReservedSeatRepository(LumiereContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ReservedSeat reservedSeat)
        {
            await SaveState(reservedSeat, EntityState.Added);
        }
        
        public async Task UpdateAsync(ReservedSeat reservedSeat)
        {
            await SaveState(reservedSeat, EntityState.Modified);
        }

        public async Task DeleteAsync(ReservedSeat reservedSeat)
        {
            _context.Remove(reservedSeat);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ReservedSeat> GetAll()
        {
            return _context.ReservedSeats;
        }

        public async Task<ReservedSeat> GetByIdAsync(Guid id)
        {
            return await _context.ReservedSeats.SingleOrDefaultAsync(sod => sod.Id == id);
        }

        private async Task SaveState(ReservedSeat reservedSeat, EntityState state)
        {
            _context.Entry(reservedSeat).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
