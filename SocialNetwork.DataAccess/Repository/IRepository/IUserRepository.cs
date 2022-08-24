using SocialNetwork.Models;
using SocialNetwork.Services.PaginationService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<PagedList<AppUser>> GetUsersAsync(PaginationParams paginationParams);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        void UpdateUser(AppUser appUser);
        Task<bool> SaveAllAsync();
    }
}
