using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.Images;
using HotelBooking.Contracts.Services;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.WebApi.Controllers
{
    [TestFixture]
    internal class ImageApiControllerTest : UnitTestBase
    {
        private const string UpdateProfileImageName = nameof(ImageApiController.UpdateProfileImage) + ". ";

        private ImageApiController imageApiController;
        private Mock<IImageService> imageServiceMock;

        [SetUp]
        public void TestInitialize()
        {
            imageServiceMock = MockRepository.Create<IImageService>();
            imageApiController = new ImageApiController(imageServiceMock.Object);
        }

        [TestCase(TestName = UpdateProfileImageName + "Should return JSON form of result got from roomService UpdateProfileImage method")]
        public async Task UpdateProfileImageTest()
        {
            UpdateProfileImageModel updateProfileImageModel = CreateUpdateProfileImageModel();

            UpdateImageResponseModel expected = CreateUpdateImageResponseModel();

            SetupImageServiceMock(updateProfileImageModel, expected);

            IActionResult actual = await imageApiController.UpdateProfileImage(updateProfileImageModel);

            UpdateImageResponseModel actualValue = (UpdateImageResponseModel)(actual as JsonResult).Value;

            Assert.AreEqual(actualValue.Message, expected.Message);
        }

        private void SetupImageServiceMock(UpdateProfileImageModel updateProfileImageModel, UpdateImageResponseModel expected)
            => imageServiceMock
                .Setup(service => service.UpdateProfileImageAsync(updateProfileImageModel))
                .ReturnsAsync(expected);

        private UpdateProfileImageModel CreateUpdateProfileImageModel()
            => new UpdateProfileImageModel();

        private UpdateImageResponseModel CreateUpdateImageResponseModel()
            => new UpdateImageResponseModel { IsSuccessful = true, Message = "default/profile.png" };
    }
}