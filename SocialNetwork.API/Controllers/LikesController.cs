using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DataAccess.Repository.IRepository;
using SocialNetwork.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers
{

    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepository;

        public LikesController(IUserRepository userRepository, ILikeRepository likeRepository)
        {
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        [HttpPost]
        [Route("AddLike/{username}")]
        public async Task<IActionResult> AddLike(string username)
        {
            var sourceUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.Username == username) return BadRequest("You can not like yourself");

            var userLike = await _likeRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("You already like this user");

            userLike = new Like
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        [Route("GetUserLikes/{predicate}")]
        public async Task<IActionResult> GetUserLikes(string predicate)
        {
            var sourceUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _likeRepository.GetUserLikes(predicate, sourceUserId));
        }
    }
}
