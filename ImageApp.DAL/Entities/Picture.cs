using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageApp.DAL.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string ImageFile { get; set; }
        public string? UserId { get; set; }
        public User User { get; set; }
    }
}
