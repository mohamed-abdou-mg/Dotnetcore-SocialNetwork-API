using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess;
using SocialNetwork.Models;
using SocialNetwork.Models.DTOs.Account;
using SocialNetwork.Services.JwtService;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(ApplicationDbContext dbContext, ITokenService tokenService, IMapper mapper)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is already taken");

            var user = _mapper.Map<AppUser>(registerDto);
            
            using var hmac = new HMACSHA512();

            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new UserDto()
            {
                Username = user.Username,
                token = _tokenService.CreateToken(user),
                Gender = user.Gender
            });
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return Ok(new UserDto()
            {
                Username = user.Username,
                token = _tokenService.CreateToken(user),
                Gender = user.Gender
            });
        }

        private async Task<bool> UserExists(string username)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }
    }
}
