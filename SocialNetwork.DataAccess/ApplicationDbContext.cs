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
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Like>().HasKey(l => new { l.LikedUserId, l.SourceUserId });

            builder.Entity<Like>().HasOne(l => l.SourceUser)
                .WithMany(u => u.LikedUsers)
                .HasForeignKey(l => l.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Like>().HasOne(l => l.LikedUser)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(l => l.LikedUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}