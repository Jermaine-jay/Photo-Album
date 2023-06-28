using System.ComponentModel.DataAnnotations;

namespace ImageApp.BLL.Models
{
	public class ForgotPasswordVM
	{
		[Required, DataType(DataType.EmailAddress)]
		public string Email { get; set; }
	}
}
