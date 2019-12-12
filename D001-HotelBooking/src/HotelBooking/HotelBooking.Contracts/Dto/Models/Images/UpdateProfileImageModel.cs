using Microsoft.AspNetCore.Http;

namespace HotelBooking.Contracts.Dto.Models.Images
{
    public class UpdateProfileImageModel
    {
        public string Token { get; set; }

        public IFormFile Image { get; set; }
    }
}