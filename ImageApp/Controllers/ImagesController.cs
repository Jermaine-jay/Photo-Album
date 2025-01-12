using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ImageApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Route("[controller]/[action]/{id?}")]
    public class ImagesController : Controller
    {
        private readonly IUploadImageService _UploadImage;
        private readonly IPropertyService _propertyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImagesController(IUploadImageService uploadImage, IHttpContextAccessor httpContextAccessor, IPropertyService propertyService)
        {
            _UploadImage = uploadImage;
            _httpContextAccessor = httpContextAccessor;
            _propertyService = propertyService;
        }

        public IActionResult Home()
        {
            return View();
        }


		[Authorize(Roles = "User")]
		public async Task<IActionResult> Album()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _propertyService.GetUserWithPicturesAsync(userId);
            return View(model);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> NewImage()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(new AddOrUpdatePictureVM { UserId = userId });
        }


		[Authorize(Roles = "User")]
		public async Task<IActionResult> UpdateImage(string? pictureId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var picture = await _propertyService.GetPicture(userId, pictureId);
            return View(picture);
        }


        [HttpGet]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> GetPicture(string pictureId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _propertyService.GetPicture(userId, pictureId);
            return View(model);
        }

		[Authorize]
		public async Task<IActionResult> AllImages()
        {
            var model = await _UploadImage.GetImages();
            return View(model);
        }


        [HttpPost]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> Save(AddOrUpdatePictureVM model)
        {
            if (ModelState.IsValid)
            {
                var (successful, msg) = await _UploadImage.AddImage(model);

                if (successful)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Album");
                }
                TempData["ErrMsg"] = msg;
                return View("NewImage");
            }
            return View("NewImage");
        }

        [HttpPost]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> SaveUpdate(PictureVM model)
        {
            if (ModelState.IsValid)
            {
                var (successful, msg) = await _UploadImage.UpdateImage(model);
                if (successful)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Album");
                }
                TempData["ErrMsg"] = msg;
                return View("UpdateImage");
            }
            return View("UpdateImage");
        }

        [HttpGet]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> DeleteImage(string pictureId)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                var (successful, msg) = await _propertyService.DeletePictureAsync(userId, pictureId);
                if (successful)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("Album");
                }
                TempData["ErrMsg"] = msg;
                return View("Album");
            }
            return View("Album");
        }

        [HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteUserImage(string pictureId)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                var (successful, msg) = await _propertyService.DeletePictureAsync(userId, pictureId);
                if (successful)
                {
                    TempData["SuccessMsg"] = msg;
                    return RedirectToAction("AllImages");
                }
                TempData["ErrMsg"] = msg;
                return View("NewImage");
            }
            return View("NewImage");
        }
    }
}
