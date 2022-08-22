using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
