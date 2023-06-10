using ImageApp.BLL.Models;

namespace ImageApp.BLL.Interface
{
	public interface IPropertyService
	{
        Task<(bool successful, string msg)> AddOrUpdateAsync(string userId, int pictureId, AllPicturesVM allPicturesVM);
        Task<(bool successful, string msg)> DeletePictureAsync(string userId, int productId);
        Task<PictureVM> GetPicture(string userId, int pictureId);
    }
}
