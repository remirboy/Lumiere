using Lumiere.Data;
using Lumiere.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly LumiereContext _context;

        public UserRepository(UserManager<User> userManager, LumiereContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateWithoutPasswordAsync(User user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> CreateWithPasswordAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Include(i => i.ReservedSeats);
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _context.Users
                .Include(i => i.ReservedSeats)
                .SingleOrDefaultAsync(sod => sod.Id == userId);
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal currentUserClaims)
        {
            return await _context.Users
                .Include(i => i.ReservedSeats)
                .SingleOrDefaultAsync(sod => sod.UserName == currentUserClaims.Identity.Name);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }
    }
}
