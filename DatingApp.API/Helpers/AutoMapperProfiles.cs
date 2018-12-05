using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using MVCDatingApp.API.Dtos;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
            .ForMember(dest => dest.PhotoURL, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p=>p.IsMainPhoto).Url);
            })
            .ForMember(dest => dest.Age, opt => {
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            });
            CreateMap<User, UserForDetailDto>()
             .ForMember(dest => dest.PhotoURL, opt => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p=>p.IsMainPhoto).Url);    
            })
            .ForMember(dest => dest.Age, opt => {
                opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
            });
            CreateMap<Photo, PhotosForDetailedDto>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<PhotoForCreationDto, Photo>();
 
            CreateMap<Photo, PhotoForReturnDto>();
            
            CreateMap<UserForRegisterDto, User>();
        }
        
    }
}