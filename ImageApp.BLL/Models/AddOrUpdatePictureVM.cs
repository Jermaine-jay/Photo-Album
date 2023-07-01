using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ImageApp.BLL.Models
{
    public class AddOrUpdatePictureVM
    {
		public string? UserId { get; set; }
		public string? PictureId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
