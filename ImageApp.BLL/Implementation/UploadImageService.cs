using AutoMapper;
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
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<Picture> _productRepo;
		private readonly IRepository<User> _userRepo;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IPropertyService _propertyService;

		public UploadImageService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_productRepo = _unitOfWork.GetRepository<Picture>();
            _userRepo = _unitOfWork.GetRepository<User>();
			_webHostEnvironment = webHostEnvironment;
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

			AllPicturesVM img = new()
			{
				ImageFile = fileName,
				Name = model.Name,
				Description = model.Description,
			};

			var result = await _propertyService?.AddOrUpdateAsync(model.UserId, model.PictureId, img);
			return result;
		}

		public async Task<IEnumerable<UserWithPicturesVM>> GetImages()
		{
			var user = await _userRepo.GetAllAsync(include: u => u.Include(x => x.Pictures));
			return	user.Select(u => new UserWithPicturesVM
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
