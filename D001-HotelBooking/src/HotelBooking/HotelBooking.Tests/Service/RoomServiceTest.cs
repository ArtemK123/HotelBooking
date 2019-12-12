using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;
using HotelBooking.Services;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.Service
{
    [TestFixture]
    internal class RoomServiceTest : UnitTestBase
    {
        private const string GetAsyncMethodName = nameof(RoomService.GetAsync) + ". ";
        private const string SearchByFiltersMethodName = nameof(RoomService.SearchByFilters) + ". ";
        private const string GetByHotelIdAsyncMethodName = nameof(RoomService.GetByHotelIdAsync) + ". ";

        private int roomId = 1;
        private int hotelId = 1;

        private RoomService roomService;

        private Mock<IRoomRepository> roomRepositoryMock;

        [SetUp]
        public void TestInitialize()
        {
            roomRepositoryMock = MockRepository.Create<IRoomRepository>();
            roomService = new RoomService(roomRepositoryMock.Object);
        }

        [TestCase(TestName = GetAsyncMethodName + "Without arguments - should return list of all rooms")]
        public async Task GetAllAsyncTest()
        {
            IReadOnlyCollection<RoomData> expected = CreateRoomCollection();

            SetupRoomRepositoryGetAllMock(expected);

            IReadOnlyCollection<RoomData> actual = await roomService.GetAsync();

            Assert.AreEqual(actual, expected);
        }

        [TestCase(TestName = GetAsyncMethodName + "With an int argument - should return room with given id")]
        public async Task GetByIdTest()
        {
            RoomData expected = CreateRoomData();

            SetupRoomRepositoryGetByIdMock(roomId, expected);

            RoomData actual = await roomService.GetAsync(roomId);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestCase(TestName = SearchByFiltersMethodName + "Should return a collection of rooms which fits the filter")]
        public void SearchByFiltersTest()
        {
            RoomRequirements roomRequirements = CreateRoomRequirements();
            IReadOnlyCollection<RoomData> expected = CreateRoomCollection();
            SetupRoomRepositorySearchByFiltersMock(roomRequirements, expected);

            IReadOnlyCollection<RoomData> actual = roomService.SearchByFilters(roomRequirements);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetByHotelIdAsyncMethodName + "Should return a list of rooms in the hotel")]
        public async Task GetByHotelIdAsyncTest()
        {
            IReadOnlyCollection<RoomData> expected = CreateRoomCollection();
            SetupRoomRepositoryGetByHotelIdAsyncMock(hotelId, expected);

            IReadOnlyCollection<RoomData> actual = await roomService.GetByHotelIdAsync(hotelId);

            Assert.AreEqual(expected, actual);
        }

        private void SetupRoomRepositoryGetByIdMock(int id, RoomData roomData)
            => roomRepositoryMock
                .Setup(repository => repository.GetAsync(id))
                .ReturnsAsync(roomData);

        private void SetupRoomRepositoryGetAllMock(IReadOnlyCollection<RoomData> roomDatas)
            => roomRepositoryMock
                .Setup(repository => repository.GetAsync())
                .ReturnsAsync(roomDatas);

        private void SetupRoomRepositorySearchByFiltersMock(RoomRequirements roomRequirements, IReadOnlyCollection<RoomData> roomDatas)
            => roomRepositoryMock
                .Setup(repository => repository.SearchByFilter(roomRequirements))
                .Returns(roomDatas);

        private void SetupRoomRepositoryGetByHotelIdAsyncMock(int hotelId, IReadOnlyCollection<RoomData> roomDatas)
            => roomRepositoryMock
                .Setup(repository => repository.GetByHotelIdAsync(hotelId))
                .ReturnsAsync(roomDatas);

        private RoomRequirements CreateRoomRequirements()
            => new RoomRequirements { PriceFrom = 10, PriceTo = 20 };

        private IReadOnlyCollection<RoomData> CreateRoomCollection()
            => new List<RoomData>();

        private RoomData CreateRoomData()
            => new RoomData { Id = roomId };

    }
}