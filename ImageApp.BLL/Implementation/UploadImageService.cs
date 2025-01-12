using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ImageApp.BLL.Implementation
{
	public class UploadImageService : IUploadImageService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<Picture> _productRepo;
		private readonly IRepository<User> _userRepo;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IPropertyService _propertyService;

		public UploadImageService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IPropertyService propertyService)
		{
			_unitOfWork = unitOfWork;
			_productRepo = _unitOfWork.GetRepository<Picture>();
			_userRepo = _unitOfWork.GetRepository<User>();
			_webHostEnvironment = webHostEnvironment;
			_propertyService = propertyService;
		}

		public async Task<(bool successful, string msg)> AddImage(AddOrUpdatePictureVM model)
		{
			var fileName = model.ImageFile.FileName;
			var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "portfolio");

			if (!Directory.Exists(imagePath))
			{
				Directory.CreateDirectory(imagePath);
			}
			string picPath = Path.Combine(imagePath, fileName);
			using (var stream = new FileStream(picPath, FileMode.Create))
			{
				await model.ImageFile.CopyToAsync(stream);
			}

            PictureVM img = new()
			{
				ImageFile = fileName,
				Name = model.Name,
				Description = model.Description,
				UserId = model.UserId,
				PictureId = model.PictureId
			};

			var result = await _propertyService?.AddOrUpdateAsync(img);
			return result;
		}

        public async Task<(bool successful, string msg)> UpdateImage(PictureVM model)
		{
            User? user = await _userRepo.GetSingleByAsync(u => u.Id == model.UserId);
            if (user == null)
            {
                return (false, $"User with id:{user.UserName} wasn't found");
            }

            var result = await _propertyService?.AddOrUpdateAsync(model);
			return result;
        }

        public async Task<IEnumerable<UserWithPicturesVM>> GetImages()
		{
			var user = await _userRepo.GetAllAsync(include: u => u.Include(x => x.Pictures));
			return user.Select(u => new UserWithPicturesVM
			{
				Id = u.Id,
				UserName = u.UserName,
				Gender = u.Gender.ToString(),
				Pictures = u.Pictures.Select(x => new Picture
				{
					Id = x.Id,
					Name = x.Name,
					UserId = x.UserId,
				})
			});
		}
	}
}
