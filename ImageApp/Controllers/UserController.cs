using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.Controllers
{
    
    [Route("[controller]/[action]/{id?}")]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
                _userServices = userServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        
        public IActionResult RegisterUser()
        {
            return View(new RegisterVM());
        }
        

        public IActionResult RegisterAdmin()
        {
            return View(new RegisterVM());
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
                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrMsg"] = msg;
                return View("Index", "Home");
            }
            return View("Index", "Home");
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
                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrMsg"] = msg;
                return View("Index", "Home");
            }
            return View("Index", "Home");
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
                    return RedirectToAction("Index", "Home");
                }

                
                TempData["ErrMsg"] = msg;
                return View("SignIn");
            }
            return View("SignIn");
        }

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
                return View("Index", "Home");
            }
            return View("Index", "Home");
        }
    }
}
