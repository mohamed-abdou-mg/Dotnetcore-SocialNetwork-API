﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DataAccess;
using System.Threading.Tasks;

namespace SocialNetwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public BugsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public IActionResult auth()
        {
            return Unauthorized();
        }    
        
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            var obj = _dbContext.Users.Find(-1);
            if (obj == null) return NotFound();
            return Ok(obj);
        }        
        
        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            var obj = _dbContext.Users.Find(-1);
            var obj2 = obj.ToString();
            return Ok();
        }       
        
        [HttpGet("bad-request")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("This wasn't a good request");
        }
    }
}
