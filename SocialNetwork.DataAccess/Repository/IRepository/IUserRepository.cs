using SocialNetwork.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        void UpdateUser(AppUser appUser);
        Task<bool> SaveAllAsync();
    }
}
