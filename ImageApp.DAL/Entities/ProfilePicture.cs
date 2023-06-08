using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.DAL.Entities
{
    public class ProfilePicture
    {
        [Key]
        public string? Id { get; set; }
        public string? ProfileImagePath { get; set; } = "Blank Pfp.jpeg";

		[ForeignKey("User")]
		public string UserId { get; set; }
        public User User { get; set; }
    }
}
