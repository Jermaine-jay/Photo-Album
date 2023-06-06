using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.DAL.DataBase
{
   
        public static class RoleSeeder
        {
            public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
            {
                if (!await roleManager.RoleExistsAsync(Roles.Admin))
                {
                    var role = new IdentityRole(Roles.Admin);
                    await roleManager.CreateAsync(role);
                }

                if (!await roleManager.RoleExistsAsync(Roles.User))
                {
                    var role = new IdentityRole(Roles.User);
                    await roleManager.CreateAsync(role);
                }
            }
        }
    
}
