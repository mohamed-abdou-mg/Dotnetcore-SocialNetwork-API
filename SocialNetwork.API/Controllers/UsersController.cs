using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DataAccess.Repository.IRepository;
using SocialNetwork.Models;
using SocialNetwork.Models.DTOs.Photo;
using SocialNetwork.Models.DTOs.User;
using SocialNetwork.Services.CloudinaryService;
using SocialNetwork.Services.PaginationService;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationParams paginationParams)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.FindFirst(ClaimTypes.Name)?.Value);

            paginationParams.currentUsername = user.Username;
            if(string.IsNullOrEmpty(paginationParams.Gender))
                paginationParams.Gender = user.Gender == "male" ? "female" : "male";

            var usersDb = await _userRepository.GetUsersAsync(paginationParams);
            var usersDto = _mapper.Map<IEnumerable<AppUserDto>>(usersDb);
            Response.AddPaginationHeader(usersDb.CurrentPage, usersDb.PageSize, usersDb.TotalCount, usersDb.TotalPages);
            return Ok(usersDto);
        }

        [HttpGet("{username}", Name ="GetUser")]
        public async Task<IActionResult> GetUser(string username)
        {
            var userDb = await _userRepository.GetUserByUsernameAsync(username);
            var userDto = _mapper.Map<AppUserDto>(userDb);
            return Ok(userDto);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateAppUserDto updateAppUserDto)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);
            _mapper.Map(updateAppUserDto, user);
            _userRepository.UpdateUser(user);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPost]
        [Route("AddPhoto")]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
                return CreatedAtRoute("GetUser", new { username = user.Username }, _mapper.Map<PhotoDto>(photo));

            return BadRequest("Problem in adding photo");
        }

        [HttpPost]
        [Route("SetMainPhoto/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);
            
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
            if (photo.IsMain) return BadRequest("There is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");            
        }

        [HttpDelete]
        [Route("DeletePhoto/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            
            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}