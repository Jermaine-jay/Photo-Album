using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.BLL.Models
{
	public class ProfilePictureVM
	{
		public string? UserId { get; set; }
		public IFormFile? ProfileImagePath { get; set; }
	}
}
