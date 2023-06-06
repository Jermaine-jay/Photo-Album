using AutoMapper;
using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ImageApp.BLL.Implementation
{
    public class UploadImageService : IUploadImageService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Pictures> _productRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadImageService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productRepo = _unitOfWork.GetRepository<Pictures>();
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<(bool successful, string msg)> AddImage(ProductVM model)
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

            AllProductVM img = new()
            {
                ImageFile = fileName,
                Name = model.Name,
                Description = model.Description,
            };

            var product = _mapper.Map<Pictures>(img);
            var rowChanges = await _productRepo.AddAsync(product);

            return rowChanges != null ? (true, "Product created successfully!") : (false, "Failed to create product");
        }


        public async Task<IEnumerable<AllProductVM>> GetImages()
        {
            var product = await _productRepo.GetAllAsync();
            var productViewModels = product.Select(u => new AllProductVM
            {
                Name = u.Name,
                Description = u.Description,
                ImageFile = u.ImageFile,
            });
            return productViewModels;
        }
    }
}
