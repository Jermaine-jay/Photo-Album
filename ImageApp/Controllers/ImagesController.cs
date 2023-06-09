using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.Controllers
{
	//[AutoValidateAntiforgeryToken]
	[Route("[controller]/[action]/{id?}")]
	public class ImagesController : Controller
	{
		private readonly IUploadImageService _UploadImage;
		public ImagesController(IUploadImageService uploadImage)
		{
			_UploadImage = uploadImage;
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

	}
}
