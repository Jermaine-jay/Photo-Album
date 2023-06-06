using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace ImageApp.BLL.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        private RoleManager<IdentityRole> _roleManager { get; }

        public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<(bool successful, string msg)> RegisterAdmin(RegisterVM register)
        {
            User newUser = new User
            {

                Email = register.Email,
                UserName = register.Username,
                Address = register.Address,
                PhoneNumber = register.PhoneNumber
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);

           /* if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }*/
            await _userManager.AddToRoleAsync(newUser, "Admin");
           /* await _signInManager.SignInAsync(newUser, isPersistent: false);*/

            return result.Succeeded ? (true, "Admin created successfully!") : (false, "Failed to create Admin");
        }

        public async Task<(bool successful, string msg)> RegisterUser(RegisterVM register)
        {
            User newUser = new User
            {
                Email = register.Email,
                UserName = register.Username,
                Address = register.Address,
                PhoneNumber = register.PhoneNumber
            };
            IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);

            /*if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }*/
            await _userManager.AddToRoleAsync(newUser, "User");


            await _signInManager.SignInAsync(newUser, isPersistent: false);

            return result.Succeeded ? (true, "User and role created successfully!") : (false, "Failed to create User and role");
        }

        public async Task<(bool successful, string msg)> SignIn(SignInVM signIn)
        {
            User user;
            if (signIn.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(signIn.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(signIn.UsernameOrEmail);
            }

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberMe, true);

                return result.Succeeded ? (true, $"{user.UserName} logged in successfully!") : (false, "Failed to login");
            }
            return (false, "User does not exist");
        }

        public async Task<(bool successful, string msg)> SignOut()
        {
            await _signInManager.SignOutAsync();
            return (true, $"logged out successfully!");
        }
    }
}
