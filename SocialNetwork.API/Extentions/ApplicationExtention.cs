using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.DataAccess.Repository;
using SocialNetwork.DataAccess.Repository.IRepository;
using SocialNetwork.Services.AutoMapperService;
using SocialNetwork.Services.JwtService;

namespace SocialNetwork.API.Extentions
{
    public static class ApplicationExtention
    {
        public static IServiceCollection AddApplicationExtention(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            return services;
        }
    }
}
