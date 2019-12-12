using System;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Models.Images;
using HotelBooking.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageApiController : Controller
    {
        private readonly IImageService imageService;

        public ImageApiController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [Route("account/update")]
        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateProfileImageModel updateProfileImageModel)
            => Json(await imageService.UpdateProfileImageAsync(updateProfileImageModel));
    }
}