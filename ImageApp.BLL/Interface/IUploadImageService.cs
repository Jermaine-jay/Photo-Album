using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Http;

namespace ImageApp.BLL.Interface
{
    public interface IUploadImageService
    {
        Task<(bool successful, string msg)> AddImage(AddOrUpdatePictureVM product);
        Task<(bool successful, string msg)> UpdateImage(PictureVM model);
        Task<IEnumerable<UserWithPicturesVM>> GetImages();
    }
}
