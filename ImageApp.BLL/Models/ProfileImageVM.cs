using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ImageApp.BLL.Models
{
    public class ProfileImageVM
    {
        [Required(ErrorMessage = "Please select a profile image.")]
        [RegularExpression(@"^.*\.(jpeg|jpg|png|JPG)$", ErrorMessage = "Only .jpeg, .jpg, and .png files are allowed.")]
        public IFormFile ProfileImagePath { get; set; }
    }
}