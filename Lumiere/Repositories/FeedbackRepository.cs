using Lumiere.Data;
using Lumiere.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly LumiereContext _context;

        public FeedbackRepository(LumiereContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(FilmFeedback feedback)
        {
            await SaveState(feedback, EntityState.Added);
        }

        public async Task UpdateAsync(FilmFeedback feedback)
        {
            await SaveState(feedback, EntityState.Modified);
        }

        public async Task DeleteAsync(FilmFeedback feedback)
        {
            _context.Remove(feedback);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<FilmFeedback> GetAll()
        {
            return _context.FilmFeedbacks;
        }

        public async Task<FilmFeedback> GetByIdAsync(Guid id)
        {
            return await _context.FilmFeedbacks.SingleOrDefaultAsync(sod => sod.Id == id);
        }

        private async Task SaveState(FilmFeedback feedback, EntityState state)
        {
            _context.Entry(feedback).State = state;
            await _context.SaveChangesAsync();
        }
    }
}
