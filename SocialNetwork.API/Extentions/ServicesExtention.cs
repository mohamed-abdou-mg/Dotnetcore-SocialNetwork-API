using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Services.AutoMapperService;
using SocialNetwork.Services.CloudinaryService;
using SocialNetwork.Services.JwtService;

namespace SocialNetwork.API.Extentions
{
    public static class ServicesExtention
    {
        public static IServiceCollection AddServicesExtention(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ITokenService, TokenService>();

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            return services;
        }
    }
}
