using SocialNetwork.Models.DTOs.Photo;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Models.DTOs.User
{
    public class UpdateAppUserDto
    {
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
