using ImageApp.DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace ImageApp.DAL.Entities
{
    public class User : IdentityUser
    {
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IList<Pictures>? Pictures { get; set; }
    }
}
