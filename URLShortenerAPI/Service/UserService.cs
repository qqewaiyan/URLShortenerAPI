using Microsoft.EntityFrameworkCore;
using URLShortenerAPI.Entity;

namespace URLShortenerAPI.Service
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserAccount>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserAccount?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserAccount> CreateAsync(UserAccount user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
