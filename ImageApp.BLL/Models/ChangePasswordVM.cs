using System.ComponentModel.DataAnnotations;

namespace ImageApp.BLL.Models
{
    public class ChangePasswordVM
    {
        public string UserId { get; set; }

        [Required, DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password), Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
