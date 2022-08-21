using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Services.JwtService;

namespace SocialNetwork.API.Extentions
{
    public static class ApplicationExtention
    {
        public static IServiceCollection AddApplicationExtention(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
