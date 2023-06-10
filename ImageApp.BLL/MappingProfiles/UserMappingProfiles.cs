using AutoMapper;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;

namespace ImageApp.BLL.MappingProfiles
{
    internal class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
            CreateMap<UserVM, User>();
            CreateMap<User, UserVM>();

            CreateMap<RegisterVM, User>();
            CreateMap<User, RegisterVM>();

            //CreateMap<UserWithPicturesVM, User>();
            CreateMap<User, UserWithPicturesVM>().ReverseMap();
        }
    }
}
