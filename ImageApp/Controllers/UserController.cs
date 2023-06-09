using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ImageApp.Controllers
{

	[Route("[controller]/[action]/{id?}")]
	public class UserController : Controller
	{
		private readonly IUserServices _userServices;
		private readonly SignInManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserController(IUserServices userServices, SignInManager<User> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_userServices = userServices;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Profile()
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userServices.GetUser(userId);
			if (user == null)
			{
				return View(new RegisterVM());

			}
			return View(user);
		}

		public IActionResult RegisterUser()
		{

			return View(new RegisterVM());

		}

		public IActionResult RegisterAdmin()
		{

			return View(new RegisterVM());

		}

		public async Task<IActionResult> UpdateUser()
		{
			var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userServices.GetUser(userId);
			if(user == null)
			{
				return View(new RegisterVM());

			}
			
			/*_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name));*/
			return View(user);


		}


		public IActionResult SignIn()
		{
			return View(new SignInVM());
		}

		[HttpPost]
		public async Task<IActionResult> SaveUser(RegisterVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _userServices.RegisterUser(model);
				if (successful)
				{

					TempData["SuccessMsg"] = msg;
					return RedirectToAction("SignIn");
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
				var (successful, msg) = await _userServices.RegisterAdmin(model);
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("SignIn");
				}
				TempData["ErrMsg"] = msg;
				return View("RegisterAdmin");
			}
			return View("RegisterAdmin");
		}

		[HttpPut]
		public async Task<IActionResult> SaveUpdate(RegisterVM model)
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
				return View("Profile");
			}
			return View("Profile");
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
				return View("Profile");
			}
			return View("Profile");
		}

		public async Task<IActionResult> SignOut()
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _userServices.SignOut();
				if (successful)
				{
					TempData["SuccessMsg"] = msg;
					return RedirectToAction("Index", "Home");
				}
				TempData["ErrMsg"] = msg;
				return View("Index", "Home");
			}
			return View("Index", "Home");
		}
	}
}
