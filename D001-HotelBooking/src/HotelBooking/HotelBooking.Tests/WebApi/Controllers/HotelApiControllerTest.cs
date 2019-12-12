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
    internal class HotelApiControllerTest : UnitTestBase
    {
        private const string GetMethodName = nameof(HotelApiController.Get) + ". ";
        private const int HotelId = 1;
        private const int SearchCount = 1;

        private Mock<IHotelService> hotelServiceMock;

        private HotelApiController hotelApiController;

        [SetUp]
        public void TestInitialize()
        {
            hotelServiceMock = MockRepository.Create<IHotelService>();
            hotelApiController = new HotelApiController(hotelServiceMock.Object);
        }

        [TestCase(TestName = GetMethodName + "Should return JSON form of result got from hotelService GetAsync method")]
        public async Task GetTest()
        {
            IReadOnlyCollection<HotelData> hotelsData = CreateHotelsData();

            SetupHotelServiceGetMock(hotelsData);

            var actual = (JsonResult)await hotelApiController.Get();

            Assert.AreEqual(hotelsData, actual.Value);
        }

        [TestCase(TestName = GetMethodName + "Should return JSON form of result got from hotelService GetAsync method with id argument")]
        public async Task GetByIdTest()
        {
            HotelData hotelData = CreateHotelData();

            SetupHotelServiceGetMock(hotelData, HotelId);

            var actual = (JsonResult)await hotelApiController.Get(HotelId);

            Assert.AreEqual(hotelData, actual.Value);
        }

        [TestCase(TestName = "Count. Should return JSON form of result got from hotelService CountSearchResults method")]
        public void CountTest()
        {
            DataForSearch dataForSearch = CreateDataForSearch();

            SetupHotelServiceCountMock(dataForSearch, SearchCount);

            var actual = (JsonResult)hotelApiController.Count(dataForSearch);

            Assert.AreEqual(SearchCount, actual.Value);
        }

        [TestCase(TestName = "Search. Should return JSON form of result got from hotelService GetSearchResultByPage method")]
        public void SearchTest()
        {
            DataForSearch dataForSearch = CreateDataForSearch();
            IReadOnlyCollection<HotelData> hotelsData = CreateHotelsData();

            SetupHotelServiceGetSearchResultMock(dataForSearch, hotelsData);

            var actual = (JsonResult)hotelApiController.Search(dataForSearch);

            Assert.AreEqual(hotelsData, actual.Value);
        }

        private void SetupHotelServiceGetMock(IReadOnlyCollection<HotelData> hotelsData)
            => hotelServiceMock
                .Setup(service => service.GetAsync())
                .ReturnsAsync(hotelsData);

        private void SetupHotelServiceGetMock(HotelData hotelData, int id)
            => hotelServiceMock
                .Setup(service => service.GetAsync(id))
                .ReturnsAsync(hotelData);

        private void SetupHotelServiceCountMock(DataForSearch dataForSearch, int count)
            => hotelServiceMock
                .Setup(service => service.CountSearchResults(dataForSearch))
                .Returns(count);

        private void SetupHotelServiceGetSearchResultMock(
            DataForSearch dataForSearch,
            IReadOnlyCollection<HotelData> hotelsData)
                => hotelServiceMock
                    .Setup(service => service.GetSearchResultByPage(dataForSearch))
                    .Returns(hotelsData);

        private IReadOnlyCollection<HotelData> CreateHotelsData() => new List<HotelData>();

        private HotelData CreateHotelData() => new HotelData();

        private DataForSearch CreateDataForSearch() => new DataForSearch();
    }
}