using ImageApp.BLL.Models;
using Microsoft.AspNetCore.Http;

namespace ImageApp.BLL.Interface
{
    public interface IUploadImageService
    {
        Task<(bool successful, string msg)> AddImage(AddOrUpdatePictureVM product);
        Task<IEnumerable<AllPicturesVM>> GetImages();
    }
}
