using ImageApp.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageApp.DAL.Entities
{
    public class User : IdentityUser
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Age { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IList<Picture>? Pictures { get; set; }
        public string? ProfilePictureId { get; set; }
        public ProfilePicture? ProfilePicture { get; set; }
    }
}
