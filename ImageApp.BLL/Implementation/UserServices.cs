using AutoMapper;
using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImageApp.BLL.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        private RoleManager<IdentityRole> _roleManager { get; }

        public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _userRepo = _unitOfWork?.GetRepository<User>();
            _mapper = mapper;
        }

        public async Task<(bool successful, string msg)> RegisterAdmin(RegisterVM register)
        {
            var newUser = CreateAUser(register);
            IdentityResult result = await _userManager.CreateAsync(newUser.Result, register.Password);

            await _userManager.AddToRoleAsync(newUser.Result, "Admin");
            await _signInManager.SignInAsync(newUser.Result, isPersistent: false);

            return result.Succeeded ? (true, "Admin created successfully!") : (false, "Failed to create Admin");
        }

        public async Task<(bool successful, string msg)> RegisterUser(RegisterVM register)
        {
            var newUser = CreateAUser(register);
            IdentityResult result = await _userManager.CreateAsync(newUser.Result, register.Password);

            await _userManager.AddToRoleAsync(newUser.Result, "User");
            await _signInManager.SignInAsync(newUser.Result, isPersistent: false);

            return result.Succeeded ? (true, "User created successfully!") : (false, "Failed to create User");
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
        public async Task<(bool successful, string msg)> Update(UserVM model)
        {

            var user = await _userRepo.GetSingleByAsync(u => u.Id == model.Id);
            if (user == null)
            {
                return (false, $"User with ID:{model.Id} wasn't found");
            }

            var userupdate = _mapper.Map(model, user);
            var rowChanges = await _userRepo.UpdateAsync(userupdate);

            return rowChanges != null ? (true, $"User detail update was successful!") : (false, "Failed To save changes!");

        }
        public async Task<(bool successful, string msg)> DeleteAsync(string userId)
        {
            var user = await _userRepo.GetSingleByAsync(u => u.Id == userId);
            if (user == null)
            {
                return (false, $"User with user:{user.UserName} wasn't found");
            }

            await _userRepo.DeleteAsync(user);
            return await _unitOfWork.SaveChangesAsync() >= 0 ? (true, $"{user.UserName} was deleted") : (false, $"Delete Failed");
        }

        public async Task<IEnumerable<PictureVM>> GetUserWithPicturesAsync(string userId)
        {
            User? user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.Pictures), tracking: true);
            IEnumerable<PictureVM>? UwT = user?.Pictures?.Select(t => new PictureVM
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                ImageFile = t.ImageFile,
            });
            return UwT;
        }

        public async Task<User> CreateAUser(RegisterVM register)
        {
            string[] dateComponents = register.DateOfBirth.Split('-');
            int year = int.Parse(dateComponents[0]);
            int age = DateTime.Now.Year - year;

            UserVM userVM = new UserVM()
            {
                UserName = register.Username,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                Address = register.Address,
                Gender = register.Gender,
                DateOfBirth = register.DateOfBirth,
                Age = age.ToString(),

            };
            var newUser = _mapper.Map<User>(userVM);

            return newUser;
        }

        public async Task<UserWithPicturesVM> UserProfileAsync(string? userId)
        {
            var u = await _userRepo.GetSingleByAsync(u => u.Id == userId);
            var useres = new UserWithPicturesVM()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                Age = u.Age,
                Gender = u.Gender.ToString(),
                DateOfBirth = u.DateOfBirth.ToString("d"),
                //ProfilePicture = u.ProfilePicture.ProfileImagePath

            };
            return useres;
        }

        public async Task<UserVM> GetUser(string? userId)
        {
            var user = await _userRepo.GetSingleByAsync(u => u.Id == userId);
            var Auser = _mapper.Map<UserVM>(user);
            return Auser;
        }
        public async Task<IEnumerable<UserVM>> GetUsers()
        {
            var users = await _userRepo.GetAllAsync();
            var userViewModels = users.Select(u => new UserVM
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                DateOfBirth = u.DateOfBirth.ToString("d"),
                Gender = u.Gender.ToString(),
                Age = u.Age,
                PhoneNumber = u.PhoneNumber,
            });
            return userViewModels;
        }
    }
}
