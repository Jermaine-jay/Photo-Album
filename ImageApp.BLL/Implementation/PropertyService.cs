using AutoMapper;
using ImageApp.BLL.Interface;
using ImageApp.BLL.Models;
using ImageApp.DAL.Entities;
using ImageApp.DAL.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ImageApp.BLL.Implementation
{
    public class PropertyService : IPropertyService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Picture> _pictureRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PropertyService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _pictureRepo = _unitOfWork.GetRepository<Picture>();
            _userRepo = _unitOfWork.GetRepository<User>();
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<(bool successful, string msg)> AddOrUpdateAsync(string userId, string pictureId, AllPicturesVM allPicturesVM)
        {
            User? user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.Pictures), tracking: true);
            if (user == null)
            {
                return (false, $"User with id:{userId} wasn't found");
            }

            Picture? picture = user.Pictures?.SingleOrDefault(t => t.Id == pictureId);
            if (picture != null)
            {
                var update = _mapper.Map(allPicturesVM, picture);
                await _pictureRepo.AddAsync(update);
                return (true, "Updated Successfully!");
            }

            var newpic = _mapper.Map<Picture>(allPicturesVM);
            user.Pictures?.Add(newpic);

            var rowChanges = await _userRepo.AddAsync(user);
            return rowChanges != null ? (true, $"Picture uccessfully created!") : (false, "Failed To Create picture!");
        }

        public async Task<(bool successful, string msg)> DeletePictureAsync(string userId, string pictureId)
        {
            User user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.Pictures), tracking: true);
            if (user == null)
            {
                return (false, $"User with ID{user?.Id} not found");
            }

            Picture? picture = user?.Pictures?.SingleOrDefault(u => u.Id == pictureId);
            if (picture != null)
            {
                await _pictureRepo.DeleteAsync(picture);
                return (true, $"task with taskId{pictureId} Deleted");
            }

            string? fileName = picture?.ImageFile;
            var filePathToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "img", "portfolio", fileName);
            if (picture == null || File.Exists(filePathToDelete))
            {
                return (false, $"Aunction with user:{picture?.Id} wasn't found");
            }

            File.Delete(filePathToDelete);
            return (false, $"Task with id:{pictureId} was not found");
        }

        public async Task<PictureVM> GetPicture(string userId, string pictureId)
        {
            User? user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.Pictures), tracking: true);
            if (!string.IsNullOrEmpty(user.ToString()))
            {
                Picture? picture = user?.Pictures?.FirstOrDefault(u => u.Id == pictureId);
                if (!string.IsNullOrEmpty(picture?.ToString()))
                {
                    PictureVM UaP = new PictureVM
                    {
                        Id = picture.Id,
                        Name = picture.Name,
                        ImageFile = picture.ImageFile,
                        Description = picture.Description,
                        UserId = userId,
                    };
                    //var userTask = _mapper.Map<PictureVM>(UaP);
                    return UaP;
                }
            }
            return null;
        }

        public async Task<IEnumerable<PictureVM>> GetUserWithPicturesAsync(string userId)
        {
            User? user = await _userRepo.GetSingleByAsync(u => u.Id == userId, include: u => u.Include(x => x.Pictures), tracking: true);
            var UwT = user?.Pictures?.Select(t => new PictureVM
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                ImageFile = t.ImageFile,
                UserId = t.UserId
            });
            return UwT;
        }
    }
}
