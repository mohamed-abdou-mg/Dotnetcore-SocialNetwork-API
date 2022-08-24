using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
