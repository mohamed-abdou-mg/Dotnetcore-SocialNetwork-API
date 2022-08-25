using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess.Repository.IRepository;
using SocialNetwork.Models;
using SocialNetwork.Models.DTOs.Like;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LikeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Like> GetUserLike(int sourceId, int likedUserId)
        {
            return await _dbContext.Likes.Where(l => l.LikedUserId == likedUserId).Where(l => l.SourceUserId == sourceId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            var users = _dbContext.Users.OrderBy(u => u.Username).AsQueryable();
            var likes = _dbContext.Likes.AsQueryable();
        
            if(predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == userId);
                users = likes.Select(like => like.LikedUser);
            }

            if (predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == userId);
                users = likes.Select(like => like.SourceUser);
            }

            return await users.Select(user => new LikeDto
            {
                Username = user.Username,
                KnownAs = user.KnownAs,
                Age = GetAge(user.DateOfBirth),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Country = user.Country,
                Id = user.Id
            }).ToListAsync();;

        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _dbContext.Users.Include(u => u.LikedUsers).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public static int GetAge(DateTime DateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }

    }
}
