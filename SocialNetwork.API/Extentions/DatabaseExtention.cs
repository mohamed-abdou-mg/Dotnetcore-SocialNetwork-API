using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.DataAccess;

namespace SocialNetwork.API.Extentions
{
    public static class DatabaseExtention
    {
        public static IServiceCollection AddDatabaseExtention(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
