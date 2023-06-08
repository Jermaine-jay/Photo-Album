using ImageApp.BLL.Models;

namespace ImageApp.BLL.Interface
{
	public interface IPropertyService
	{
		Task<(bool successful, string msg)> AddOrUpdateAsync(string userId, string pictureId, AllPicturesVM allPicturesVM);
		Task<(bool successful, string msg)> DeletePictureAsync(string userId, string productId);
		Task<PictureVM> GetPicture(string userId, string pictureId);

	}
}
