using ImageApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.BLL.Jwt.Interface
{
	public interface IJwtConfigurations
	{
		public string GenerateToken(User user);
		public bool IsTokenValid(string token);
	}
}
