using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ImageApp.DAL.DataBase
{
    public class ImageAppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ImageAppDbContext(DbContextOptions<ImageAppDbContext> options)
          : base(options)
        {

        }
        public DbSet<Pictures> Products { get; set; }

        
    }
}
