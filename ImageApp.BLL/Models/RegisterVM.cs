using ImageApp.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImageApp.BLL.Models
{

    public class RegisterVM
    {
        public string? Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Address { get; set; }

		public string Gender { get; set; }

		[Required, DataType(DataType.DateTime)]
		public string DateOfBirth { get; set; }

		[Required, DataType(DataType.PhoneNumber), MaxLength(12)]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}