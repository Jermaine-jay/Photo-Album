using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.DAL.Entities
{
    public class User:IdentityUser
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
