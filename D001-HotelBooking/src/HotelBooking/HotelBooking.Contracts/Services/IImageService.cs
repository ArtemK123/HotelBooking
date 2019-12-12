using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.Images;

namespace HotelBooking.Contracts.Services
{
    public interface IImageService
    {
        Task<UpdateImageResponseModel> UpdateProfileImageAsync(UpdateProfileImageModel updateProfileImageModel);
    }
}