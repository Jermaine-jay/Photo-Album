using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ImageApp.Controllers
{
	//[AutoValidateAntiforgeryToken]
	[Route("[controller]/[action]/{id?}")]
	public class ImagesController : Controller
	{
		private readonly IUploadImageService _UploadImage;
		private readonly IPropertyService _propertyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ImagesController(IUploadImageService uploadImage, IHttpContextAccessor httpContextAccessor, IPropertyService propertyService )
		{
			_UploadImage = uploadImage;
			_httpContextAccessor = httpContextAccessor;
			_propertyService = propertyService;
		}
		public IActionResult Home()
		{
			return View();
		}
		public IActionResult Album()
		{
			return View();
		}

		/*[Authorize(Roles = Roles.User)]*/
		public IActionResult NewImage()
		{
			return View(new AddOrUpdatePictureVM());
		}

		/*[Authorize(Roles = Roles.User)]*/
		public async Task<IActionResult> AllImages()
		{
			var model = await _UploadImage.GetImages();
			return View(model);
		}


		[HttpPost]
		public async Task<IActionResult> Save(AddOrUpdatePictureVM model)
		{
			if (ModelState.IsValid)
			{
				var (successful, msg) = await _UploadImage.AddImage(model);

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


		[HttpGet("{productId}")]
        public async Task<IActionResult> DeleteImage(int productId)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                var (successful, msg) = await _propertyService.DeletePictureAsync(userId, productId);

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
