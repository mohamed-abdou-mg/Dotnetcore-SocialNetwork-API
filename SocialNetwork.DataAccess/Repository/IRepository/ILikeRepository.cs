using SocialNetwork.Models;
using SocialNetwork.Models.DTOs.Like;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repository.IRepository
{
    public interface ILikeRepository
    {
        Task<Like> GetUserLike(int sourceId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
    }
}
