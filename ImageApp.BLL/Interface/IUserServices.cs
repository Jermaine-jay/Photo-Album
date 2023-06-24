using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.BLL.Interface
{
    public interface IUserServices
    {
		Task<(bool successful, string msg)> RegisterUser(IUrlHelper urlHelper, string Protocol,RegisterVM register);
        Task<(bool successful, string msg)> RegisterAdmin(RegisterVM register);
        Task<(bool successful, string msg)> SignIn(SignInVM register);
        Task<(bool successful, string msg)> SignOut();
        Task<(bool successful, string msg)> Update(UserVM model);
        Task<(bool successful, string msg)> DeleteAsync(string userId);
        Task<ProfileVM> UserProfileAsync(string userId);
        Task<ProfileVM> GetUser(string userId);
        Task<IEnumerable<ProfileVM>> GetUsers();

    }
}
