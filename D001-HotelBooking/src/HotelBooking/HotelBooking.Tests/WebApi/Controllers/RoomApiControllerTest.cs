using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;
using HotelBooking.Contracts.Services;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.WebApi.Controllers
{
    [TestFixture]
    internal class RoomApiControllerTest : UnitTestBase
    {
        private const string GetMethodName = nameof(RoomApiController.Get) + ". ";
        private const string SearchMethodName = nameof(RoomApiController.Search) + ". ";
        private const string GetByHotelIdMethodName = nameof(RoomApiController.GetByHotelId) + ". ";
        private const int RoomId = 1;
        private const int HotelId = 3;

        private RoomApiController roomApiController;
        private Mock<IRoomService> roomServiceMock;

        [SetUp]
        public void TestInitialize()
        {
            roomServiceMock = MockRepository.Create<IRoomService>();
            roomApiController = new RoomApiController(roomServiceMock.Object);
        }

        [TestCase(TestName = GetMethodName + "Should return JSON form of result got from roomService GetAsync method")]
        public async Task GetTest()
        {
            IReadOnlyCollection<RoomData> roomsData = CreateRoomsData();

            SetupRoomServiceGetMock(roomsData);

            var actual = (JsonResult)await roomApiController.Get();

            Assert.AreEqual(roomsData, actual.Value);
        }

        [TestCase(TestName = GetMethodName + "Should return JSON form of result got from roomService GetAsync method with id argument")]
        public async Task GetByIdTest()
        {
            RoomData roomData = CreateRoomData();

            SetupRoomServiceGetMock(roomData, RoomId);

            var actual = (JsonResult)await roomApiController.Get(RoomId);

            Assert.AreEqual(roomData, actual.Value);
        }

        [TestCase(TestName = SearchMethodName + "Search. Should return JSON form of result got from roomService GetSearchResultByPage method")]
        public void SearchTest()
        {
            RoomRequirements roomRequirements = CreateRoomRequirements();
            IReadOnlyCollection<RoomData> roomsData = CreateRoomsData();

            SetupHotelServiceGetSearchResultMock(roomsData, roomRequirements);

            var actual = (JsonResult)roomApiController.Search(roomRequirements);

            Assert.AreEqual(roomsData, actual.Value);
        }

        [TestCase(TestName = GetByHotelIdMethodName + "Should return JSON form of result got from roomService GetByHotelIdAsync method with id argument")]
        public async Task GetByHotelIdTest()
        {
            IReadOnlyCollection<RoomData> roomsData = CreateRoomsData();

            SetupRoomServiceGetByHotelIdMock(roomsData, HotelId);

            var actual = (JsonResult)await roomApiController.GetByHotelId(HotelId);

            Assert.AreEqual(roomsData, actual.Value);
        }

        private void SetupRoomServiceGetByHotelIdMock(IReadOnlyCollection<RoomData> roomsData, int hotelId)
            => roomServiceMock
                .Setup(service => service.GetByHotelIdAsync(hotelId))
                .ReturnsAsync(roomsData);

        private void SetupHotelServiceGetSearchResultMock(IReadOnlyCollection<RoomData> roomsData, RoomRequirements roomRequirements)
           => roomServiceMock
               .Setup(service => service.SearchByFilters(roomRequirements))
               .Returns(roomsData);

        private void SetupRoomServiceGetMock(IReadOnlyCollection<RoomData> roomsData)
            => roomServiceMock
                .Setup(service => service.GetAsync())
                .ReturnsAsync(roomsData);

        private void SetupRoomServiceGetMock(RoomData roomsData, int roomId)
           => roomServiceMock
               .Setup(service => service.GetAsync(roomId))
               .ReturnsAsync(roomsData);

        private RoomRequirements CreateRoomRequirements()
            => new RoomRequirements();

        private IReadOnlyCollection<RoomData> CreateRoomsData()
           => new List<RoomData>();

        private RoomData CreateRoomData()
           => new RoomData();
    }
}