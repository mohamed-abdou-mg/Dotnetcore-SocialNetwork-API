using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess.Repository.IRepository;
using SocialNetwork.Models;
using SocialNetwork.Services.PaginationService;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<PagedList<AppUser>> GetUsersAsync(PaginationParams paginationParams)
        {
            var query = _dbContext.Users
                .Include(u => u.Photos).AsQueryable();

            //filteration
            
            //filter by gender
            query = query.Where(q => q.Username != paginationParams.currentUsername);
            query = query.Where(q => q.Gender == paginationParams.Gender);
            
            //filter by date of birth
            var minDob = DateTime.Today.AddYears(-paginationParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-paginationParams.MinAge);
            query = query.Where(q => q.DateOfBirth >= minDob && q.DateOfBirth <= maxDob);

            //Ordering
            query = paginationParams.OrderBy switch
            {
                "created" => query.OrderByDescending(q => q.Created),
                _ => query.OrderByDescending(q => q.LastActive)
            };

            return await PagedList<AppUser>.CreateAsync(query.AsNoTracking(), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void UpdateUser(AppUser appUser)
        {
            _dbContext.Users.Update(appUser);
        }
    }
}
