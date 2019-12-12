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
    internal class HotelServiceTest : UnitTestBase
    {
        private const string GetAsyncMethodName = nameof(HotelService.GetAsync) + ". ";
        private const string GetSearchResultByPageMethodName = nameof(HotelService.GetSearchResultByPage) + ". ";
        private const int DefaultPageSize = 20;
        private const int Page = 1;

        private Mock<IHotelRepository> hotelRepositoryMock;

        private HotelService hotelService;

        [SetUp]
        public void TestInitialize()
        {
            hotelRepositoryMock = MockRepository.Create<IHotelRepository>();
            hotelService = new HotelService(hotelRepositoryMock.Object);
        }

        [TestCase(TestName = GetAsyncMethodName + "Should return result got from hotelRepository GetAsync method")]
        public async Task GetAsyncTest()
        {
            IReadOnlyCollection<HotelData> expected = CreateHotelsData();
            SetupHotelRepositoryGetAsyncMock(expected);

            IReadOnlyCollection<HotelData> actual = await hotelService.GetAsync();

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName =
            "CountSearchResults. should return result got from hotelRepository CountSearchResults method")]
        public void CountSearchResultsTest()
        {
            DataForSearch dataForSearch = CreateDataForSearch();
            int expected = 5;

            SetupHotelRepositoryCountSearchResultsMock(dataForSearch, expected);

            int actual = hotelService.CountSearchResults(dataForSearch);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetSearchResultByPageMethodName +
                             "Should return result got from hotelRepository GetSearchResultByPage method when dataForSearch is not null")]
        public void GetSearchResultByPageTest()
        {
            DataForSearch dataForSearch = CreateDataForSearch();
            IReadOnlyCollection<HotelData> expected = CreateHotelsData();

            SetupHotelRepositoryGetSearchResultByPageMock(dataForSearch, expected);

            IReadOnlyCollection<HotelData> actual = hotelService.GetSearchResultByPage(dataForSearch);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetSearchResultByPageMethodName +
            "Should return result got from hotelRepository GetByPage method when dataForSearch is null")]
        public void GetSearchResultByPageNullDataForSearchTest()
        {
            IReadOnlyCollection<HotelData> expected = CreateHotelsData();

            SetupHotelRepositoryGetByPageMock(Page, expected);

            IReadOnlyCollection<HotelData> actual = hotelService.GetSearchResultByPage(null);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetAsyncMethodName + " With id argument should return result got from hotelRepository GetAsync(id) method")]
        public async Task GetAsyncByIdTest()
        {
            int hotelId = 1;
            HotelData expected = CreateHotelData();

            SetupHotelRepositoryGetAsyncMock(hotelId, expected);

            HotelData actual = await hotelService.GetAsync(hotelId);

            Assert.AreEqual(expected, actual);
        }

        private void SetupHotelRepositoryGetAsyncMock(IReadOnlyCollection<HotelData> hotelsData)
            => hotelRepositoryMock
                .Setup(repository => repository.GetAsync())
                .ReturnsAsync(hotelsData);

        private void SetupHotelRepositoryCountSearchResultsMock(DataForSearch dataForSearch, int expectedCount)
            => hotelRepositoryMock
                .Setup(repository => repository.CountSearchResults(dataForSearch))
                .Returns(expectedCount);

        private void SetupHotelRepositoryGetSearchResultByPageMock(
            DataForSearch dataForSearch,
            IReadOnlyCollection<HotelData> hotelsData)
                => hotelRepositoryMock
                    .Setup(repository => repository.GetSearchResultByPage(dataForSearch, DefaultPageSize))
                    .Returns(hotelsData);

        private void SetupHotelRepositoryGetByPageMock(
            int page,
            IReadOnlyCollection<HotelData> hotelsData)
            => hotelRepositoryMock
                .Setup(repository => repository.GetByPage(page, DefaultPageSize))
                .Returns(hotelsData);

        private void SetupHotelRepositoryGetAsyncMock(int id, HotelData hotelData)
            => hotelRepositoryMock
                .Setup(repository => repository.GetAsync(id))
                .ReturnsAsync(hotelData);

        private IReadOnlyCollection<HotelData> CreateHotelsData() => new List<HotelData>();

        private DataForSearch CreateDataForSearch() => new DataForSearch();

        private HotelData CreateHotelData() => new HotelData();
    }
}