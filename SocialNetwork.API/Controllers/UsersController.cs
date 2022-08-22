using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess;
using SocialNetwork.DataAccess.Repository.IRepository;
using SocialNetwork.Models.DTOs.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var usersDb = await _userRepository.GetUsersAsync();
            var usersDto = _mapper.Map<IEnumerable<AppUserDto>>(usersDb);
            return Ok(usersDto);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var userDb = await _userRepository.GetUserByUsernameAsync(username);
            var userDto = _mapper.Map<AppUserDto>(userDb);
            return Ok(userDto);
        }

    }
}
