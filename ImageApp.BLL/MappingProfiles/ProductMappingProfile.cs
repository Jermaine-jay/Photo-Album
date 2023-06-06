using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ImageApp.DAL.Entities;
using ImageApp.BLL.Models;

namespace ImageApp.BLL.MappingProfiles
{
    public class ProductMappingProfile:Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductVM, Pictures>();
            CreateMap<Pictures, ProductVM>();
            CreateMap<AllProductVM, Pictures>();
        }
    }
}
