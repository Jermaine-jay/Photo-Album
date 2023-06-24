using System.ComponentModel.DataAnnotations;

namespace ImageApp.BLL.Models
{
	public class ResetPasswordVM
	{
		[Required, DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		public string Code { get; set; }


		[Required, DataType(DataType.Password)]
		public string Password { get; set; }


		[DataType(DataType.Password), Compare(nameof(Password))]
		public string ConfirmPassword { get; set; }
	}
}
