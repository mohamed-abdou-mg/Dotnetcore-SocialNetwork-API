using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models.DTOs.Photo
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}