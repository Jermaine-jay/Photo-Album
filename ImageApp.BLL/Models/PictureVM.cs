using ImageApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.BLL.Models
{
	public class PictureVM
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public string? ImageFile { get; set; }
        public string? UserId { get; set; }

    }
}
