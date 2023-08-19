using AutoMapper;
using ImageApp.BLL.Implementation;
using ImageApp.BLL.Interface;
using ImageApp.BLL.Jwt.Interface;
using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;

namespace ImageApp.Controllers
{

	[Route("[controller]/[action]/{id?}")]
	public class UserController : Controller
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IServiceFactory _serviceFactory;
	

		public UserController(IServiceFactory serviceFactory, IHttpContextAccessor httpContextAccessor)
		{
			_serviceFactory = serviceFactory;
			_httpContextAccessor = httpContextAccessor;
		}


		public IActionResult WaitingPage()
		{
			return View();
		}


		public IActionResult ForgotPassword()
		{
			return View(new ForgotPasswordVM());
		}


		public IActionResult RegisterUser()
		{
			return View(new RegisterVM());
		}


		[Authorize(Roles = "Admin")]
		public IActionResult RegisterAdmin()
		{
			return View(new RegisterVM());
		}



		public async Task<IActionResult> Profile()
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _serviceFactory.GetService<IUserServices>().UserProfileAsync(userId);
			
			var profile = new CombinedVM()
			{
				User = user,
				Image = new ProfileImageVM(),
            };
            
            return View(profile);
        }



		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AllUsers()
		{
			var model = await _serviceFactory.GetService<IUserServices>().GetUsers();
			return View(model);
		}



		[Authorize]
		public async Task<IActionResult> UpdateUser(string? Id)
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (Id != null)
			{
				var result = await _serviceFactory.GetService<IUserServices>().GetUser(Id);
				return View(result);
			}
			var user = await _serviceFactory.GetService<IUserServices>().GetUser(Id);
			return View(userId);
		}



        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage(CombinedVM model)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                var (successful, msg) = await _serviceFactory.GetService<IUserServices>().UpdateProfileImage(model.Image, userId);
                if (successful)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Profile");
                }

                TempData["ErrMsg"] = msg;
                return View("Profile");
            }
            return View("Profile");
        }


        public IActionResult SignIn()
		{
			return View(new SignInVM());
		}



		public IActionResult ResetPassword(string? code, string userId)
		{
			if (code == null)
			{
				return RedirectToAction("Error");
			}
			var model = new ResetPasswordVM { Code = code, UserId = userId };
			return View(model);
		}



		public async Task<IActionResult> ConfirmEmail(string userId, string code)
		{
			var (successful, msg) = await _serviceFactory.GetService<IAuthenticationService>().ConfirmEmail(userId, code);
			if (successful)
			{
				TempData["SuccessMsg"] = msg;
				return RedirectToAction("SignIn");
			}
			TempData["ErrMsg"] = msg;
			return View("SignIn");
		}



		[HttpPost]
		public async Task<IActionResult> SaveUser(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IUserServices>().RegisterUser(model);
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("WaitingPage");
				}
				TempData["ErrMsg"] = msg;
				return View("RegisterUser");
			}
			return View("RegisterUser");
		}



		[HttpPost]
		public async Task<IActionResult> SaveAdmin(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IUserServices>().RegisterAdmin(model);
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("WaitingPage");
				}
				TempData["ErrMsg"] = msg;
				return View("RegisterAdmin");
			}
			return View("RegisterAdmin");
		}


		[HttpPost]
		public async Task<IActionResult> SaveUpdate(UserVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IUserServices>().Update(model);
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("Profile");
				}
				TempData["ErrMsg"] = msg;
				return View("UpdateUser");
			}
			return View("UpdateUser");
		}



		[HttpPost]
		public async Task<IActionResult> SignIn(SignInVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IUserServices>().SignIn(model);
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("Profile");
				}
				TempData["ErrMsg"] = msg;
				return View("signIn");
			}
			return View("signIn");
		}



		[Authorize]
		public async Task<IActionResult> SignOut()
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IUserServices>().SignOut();
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("SignIn");
				}
				TempData["ErrMsg"] = msg;
				return View("SignIn");
			}
			return View("SignIn");
		}



		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteUser(string userId)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IUserServices>().DeleteAsync(userId);
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("AllUsers");
				}
				TempData["ErrMsg"] = msg;
				return View("AllUsers");
			}
			return View("AllUsers");
		}



		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteMyAccount(string userId)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IUserServices>().DeleteAsync(userId);
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("SignIn");
				}
				TempData["ErrMsg"] = msg;
				return View("SignIn");
			}
			return View("SignIn");
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetUserPassword(ResetPasswordVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IRecoveryService>().ResetPassword(model);
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("SignIn");
				}
				TempData["ErrMsg"] = msg;
				return View("ResetPassword");
			}
			return View("ResetPassword");
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UserForgotPassword(ForgotPasswordVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IRecoveryService>().ForgotPassword(model);

				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("WaitingPage");
				}
				TempData["ErrMsg"] = msg;
				return View("WaitingPage");
			}
			return View("WaitingPage");

		}


		public async Task<ActionResult> ChangePassword()
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(new ChangePasswordVM { UserId = userId });
		}



		public async Task<IActionResult> ConfirmToken()
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			return View(new ConfirmTokenVM { UserId = userId });
		}



		[Authorize]
		public async Task<IActionResult> UserChangePassword()
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var (successful, msg) = await _serviceFactory.GetService<IRecoveryService>().ChangeDetailToken(userId);
			if (successful)
			{
				TempData["SuccessMsg"] = msg;
				return RedirectToAction("ConfirmToken");
			}
			TempData["ErrMsg"] = msg;
			return View("WaitingPage");
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ConfirmUserToken(ConfirmTokenVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IRecoveryService>().VerifyChangeDetailToken(model);

				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("ChangePassword");
				}
				TempData["ErrMsg"] = msg;
				return View("ConfirmToken");
			}
			return View("ConfirmToken");

		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangeUserPassword(ChangePasswordVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _serviceFactory.GetService<IRecoveryService>().ChangePassword(model);

				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("Profile");
				}
				TempData["ErrMsg"] = msg;
				return View("Confirm");
			}
			return View("WaitingPage");

		}

	}
}
