using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Data;
using System.Security.Claims;

namespace ImageApp.Controllers
{

    [Route("[controller]/[action]/{id?}")]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRecoveryService _recoveryService;

        public UserController(IUserServices userServices, IRecoveryService recoveryService, IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, IAuthenticationService authenticationService)
        {
            _userServices = userServices;
            _httpContextAccessor = httpContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _authenticationService = authenticationService;
            _recoveryService = recoveryService;
        }

        public IActionResult WaitingPage()
        {
            return View();
        }

		[Authorize]
		public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordVM());
        }


		[Authorize(Roles = "User")]
		public IActionResult RegisterUser()
        {
            return View(new RegisterVM());
        }


		[Authorize(Roles = "Admin")]
		public IActionResult RegisterAdmin()
        {
            return View(new RegisterVM());
        }


        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userServices.UserProfileAsync(userId);
            if (user == null)
            {
                return View(new ProfileVM());
            }
            return View(user);
        }


		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AllUsers()
        {
            var model = await _userServices.GetUsers();
            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> UpdateUser(string? Id)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Id != null)
            {
                var result = await _userServices.GetUser(Id);
                return View(result);
            }
            var user = await _userServices.GetUser(Id);
            return View(userId);

            /*_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name));*/
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
            var (successful, msg) = await _authenticationService.ConfirmEmail(userId, code);
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
                var urlHelper = _urlHelperFactory.GetUrlHelper(ControllerContext);
                var (successful, msg) = await _userServices.RegisterUser(urlHelper, model);
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
                var urlHelper = _urlHelperFactory.GetUrlHelper(ControllerContext);
                var (successful, msg) = await _userServices.RegisterUser(urlHelper, model);
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
                var (successful, msg) = await _userServices.Update(model);
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
                var (successful, msg) = await _userServices.SignIn(model);
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
                var (successful, msg) = await _userServices.SignOut();
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
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (ModelState.IsValid)
            {
                var (successful, msg) = await _userServices.DeleteAsync(userId);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetUserPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var (successful, msg) = await _recoveryService.ResetPassword(model);
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
                var urlHelper = _urlHelperFactory.GetUrlHelper(ControllerContext);
                var (successful, msg) = await _recoveryService.ForgotPassword(urlHelper, model);

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
    }
}
