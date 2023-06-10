using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageApp.DAL.DataBase
{
    public class ImageAppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ImageAppDbContext(DbContextOptions<ImageAppDbContext> options)
          : base(options)
        {

        }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(t1 => t1.ProfilePicture)
                .WithOne(t2 => t2.User)
                .HasForeignKey<ProfilePicture>(t2 => t2.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
               .HasMany(u => u.Pictures)
               .WithOne(i => i.User)
               .HasForeignKey(i => i.UserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
