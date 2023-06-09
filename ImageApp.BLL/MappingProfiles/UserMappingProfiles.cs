using AutoMapper;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.BLL.MappingProfiles
{
    internal class UserMappingProfiles:Profile
    {
        public UserMappingProfiles()
        {
            CreateMap<UserVM, User>();
            CreateMap<User, UserVM>();
            CreateMap<RegisterVM, User>();
            CreateMap<User, RegisterVM>();
        }
    }
}
