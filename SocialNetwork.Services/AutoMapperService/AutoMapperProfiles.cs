using AutoMapper;
using SocialNetwork.Models;
using SocialNetwork.Models.DTOs.Account;
using SocialNetwork.Models.DTOs.Photo;
using SocialNetwork.Models.DTOs.User;
using System;
using System.Linq;

namespace SocialNetwork.Services.AutoMapperService
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, AppUserDto>()
                .ForMember(udto => udto.PhotoUrl, 
                u => u.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(udto => udto.Age, u => u.MapFrom(src => GetAge(src.DateOfBirth)));

            CreateMap<Photo, PhotoDto>();

            CreateMap<UpdateAppUserDto, AppUser>();

            CreateMap<RegisterDto, AppUser>();
        }

        public int GetAge(DateTime DateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}