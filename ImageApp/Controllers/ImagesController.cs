using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageApp.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Route("[controller]/[action]/{id?}")]
    public class ImagesController : Controller
    {
        private readonly IUploadImageService _UploadImage;
        public ImagesController(IUploadImageService uploadImage)
        {
            _UploadImage = uploadImage;
        }
        /*public IActionResult Index()
        {
            return View();
        }*/

        [Authorize(Roles = Roles.User)]
        public IActionResult NewImage()
        {
            return View(new ProductVM());
        }

        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> AllImages()
        {
            var model = await _UploadImage.GetImages();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductVM model)
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
