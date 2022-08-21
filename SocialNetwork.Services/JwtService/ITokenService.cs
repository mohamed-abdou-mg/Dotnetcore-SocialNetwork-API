using SocialNetwork.Models;

namespace SocialNetwork.Services.JwtService
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
