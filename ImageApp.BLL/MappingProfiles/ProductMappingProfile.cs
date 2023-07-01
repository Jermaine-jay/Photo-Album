using AutoMapper;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;

namespace ImageApp.BLL.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<AddOrUpdatePictureVM, Picture>();
            CreateMap<Picture, AddOrUpdatePictureVM>();

            CreateMap<AllPicturesVM, Picture>();
            CreateMap<Picture, AllPicturesVM>();

            CreateMap<PictureVM, Picture>();
            CreateMap<Picture, PictureVM>();
        }
    }
}
