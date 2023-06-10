using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;

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
        Task<IEnumerable<PictureVM>> GetUserWithPicturesAsync(string userId);
        Task<UserWithPicturesVM> UserProfileAsync(string? userId);
        Task<UserVM> GetUser(string? userId);
        Task<User> CreateAUser(RegisterVM register);
        Task<IEnumerable<UserVM>> GetUsers();

    }
}
