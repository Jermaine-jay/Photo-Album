using ImageApp.BLL.Models;

namespace ImageApp.BLL.Interface
{
	public interface IPropertyService
	{
        Task<(bool successful, string msg)> AddOrUpdateAsync(PictureVM allPicturesVM);
        Task<(bool successful, string msg)> DeletePictureAsync(string userId, string productId);
        Task<PictureVM> GetPicture(string userId, string pictureId);
        Task<IEnumerable<PictureVM>> GetUserWithPicturesAsync(string userId);
    }
}
