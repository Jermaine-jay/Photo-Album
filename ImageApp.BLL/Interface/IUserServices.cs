using ImageApp.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.BLL.Interface
{
    public interface IUserServices
    {
        Task<(bool successful, string msg)> RegisterUser(RegisterVM register); 
        Task<(bool successful, string msg)> RegisterAdmin(RegisterVM register);
        Task<(bool successful, string msg)> SignIn(SignInVM register);
        Task<(bool successful, string msg)> SignOut();
    }
}
