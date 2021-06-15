using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Entities;
using v0528.Extensions;
using v0528.Models;

namespace v0528.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberModel>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest=>dest.Age,opt =>opt.MapFrom(src=>src.DateOfBirth.CalculateAge())) ;
            CreateMap<Photo, PhotoModel>();
            CreateMap<MemberUpdateModel, AppUser>();
            CreateMap<RegisterModel, AppUser>();
        }
    }
}
