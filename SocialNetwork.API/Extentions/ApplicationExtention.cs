using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.API.ActionFilters;
using SocialNetwork.DataAccess.Repository;
using SocialNetwork.DataAccess.Repository.IRepository;

namespace SocialNetwork.API.Extentions
{
    public static class ApplicationExtention
    {
        public static IServiceCollection AddApplicationExtention(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<LogUserActivity>();
            return services;
        }
    }
}
