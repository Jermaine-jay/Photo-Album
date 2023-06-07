using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.DAL.Entities
{
    public class ProfilePicture
    {
        public int? Id { get; set; }
        public string? ProfileImagePath { get; set; } = "Blank Pfp.jpeg";
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
