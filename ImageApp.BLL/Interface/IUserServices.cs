using ImageApp.BLL.Models;

namespace ImageApp.BLL.Interface
{
	public interface IUserServices
	{
		Task<(bool successful, string msg)> RegisterUser(RegisterVM register);
		Task<(bool successful, string msg)> RegisterAdmin(RegisterVM register);
		Task<(bool successful, string msg)> SignIn(SignInVM register);
		Task<(bool successful, string msg)> SignOut();
		Task<(bool successful, string msg)> Update(UserVM model);
		Task<(bool successful, string msg)> DeleteAsync(string userId);
		Task<ProfileVM> UserProfileAsync(string userId);
		Task<ProfileVM> GetUser(string userId);
		Task<IEnumerable<ProfileVM>> GetUsers();

		Task<(bool successful, string msg)> UpdateProfileImage(ProfileImageVM model, string userId);



    }
}
