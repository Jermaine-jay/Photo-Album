using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ImageApp.BLL.Models
{
    public class ProductVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Image")]
        //[FileExtensions(Extensions = "jpg,jpeg,png,gif")]
        public IFormFile ImageFile { get; set; }
    }
}
