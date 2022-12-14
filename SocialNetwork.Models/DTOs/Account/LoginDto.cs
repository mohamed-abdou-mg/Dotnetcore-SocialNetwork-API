using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models.DTOs.Account
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
