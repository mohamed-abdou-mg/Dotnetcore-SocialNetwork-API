using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess.Repository.IRepository;
using SocialNetwork.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users
                .Include(u => u.Photos)
                .SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dbContext.Users
                .Include(u => u.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void UpdateUser(AppUser appUser)
        {
            _dbContext.Entry(appUser).State = EntityState.Modified;
        }
    }
}
