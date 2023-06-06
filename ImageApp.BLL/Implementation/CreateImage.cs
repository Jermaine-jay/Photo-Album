using AutoMapper;
using ImageApp.BLL.Interface;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Http;

namespace ImageApp.BLL.Implementation
{
    public class CreateImage : ICreateImage
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Pictures> _productRepo;

        public CreateImage(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productRepo = _unitOfWork.GetRepository<Pictures>();
        }
        public async Task<string> CreateImageFile(IFormFile file)
        {
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedImages"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var ImagePath = path + $"/{file.FileName}";
            return ImagePath;

        }
    }
}
