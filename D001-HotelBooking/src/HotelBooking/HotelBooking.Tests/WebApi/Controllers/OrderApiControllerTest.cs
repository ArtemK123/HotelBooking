using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Models;
using HotelBooking.Contracts.Dto.Models.Orders;
using HotelBooking.Contracts.Services;
using HotelBooking.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.Service.Handlers
{
    [TestFixture]
    internal class OrderApiControllerTest : UnitTestBase
    {
        private const string GetByClientMethodName = nameof(OrderApiController.GetByClient) + ". ";
        private const string GetByHotelMethodName = nameof(OrderApiController.GetByHotel) + ". ";
        private const string AddMethodName = nameof(OrderApiController.Add) + ". ";

        private const int ClientId = 5;
        private const int HotelId = 2;

        private OrderApiController orderApiController;

        private Mock<IOrderService> orderServiceMock;

        [SetUp]
        public void TestInitialize()
        {
            orderServiceMock = MockRepository.Create<IOrderService>();
            orderApiController = new OrderApiController(orderServiceMock.Object);
        }

        [TestCase(TestName = GetByClientMethodName + "Should return IActionResult with all orders related to given client")]
        public async Task GetByClientTest()
        {
            IReadOnlyCollection<GetOrderModel> orderDatas = CreateEmptyOrderDataCollection();

            SetupOrderServiceGetByClientAsyncMock(ClientId, orderDatas);

            IActionResult actual = await orderApiController.GetByClient(ClientId);

            Assert.AreEqual((actual as JsonResult).Value, orderDatas);
        }

        [TestCase(TestName = GetByHotelMethodName + "Should return IActionResult with all orders related to given hotel")]
        public async Task GetByHotelTest()
        {
            IReadOnlyCollection<GetOrderModel> orderDatas = CreateEmptyOrderDataCollection();

            SetupOrderServiceGetByHotelAsyncMock(HotelId, orderDatas);

            IActionResult actual = await orderApiController.GetByHotel(HotelId);
            var actualValue = (actual as JsonResult).Value;
            Assert.AreEqual(actualValue, actualValue);
        }

        [TestCase(TestName = AddMethodName + "Should return DefaultResponseModel with the information about storing the new order")]
        public async Task AddOrderAsyncTest()
        {
            AddOrderModel addOrderModel = CreateAddOrderModel();

            DefaultResponseModel expected = CreateDefaultResponseModel();

            SetupOrderServiceAddAsyncMock(addOrderModel, expected);

            IActionResult actual = await orderApiController.Add(addOrderModel);

            DefaultResponseModel actualValue = (DefaultResponseModel)(actual as JsonResult).Value;

            Assert.AreEqual(actualValue.Message, expected.Message);
        }

        private void SetupOrderServiceGetByClientAsyncMock(int clientId, IReadOnlyCollection<GetOrderModel> orderDatas)
            => orderServiceMock
                .Setup(service => service.GetByClientAsync(clientId))
                .ReturnsAsync(orderDatas);

        private void SetupOrderServiceGetByHotelAsyncMock(int hotelId, IReadOnlyCollection<GetOrderModel> orderDatas)
            => orderServiceMock
                .Setup(service => service.GetByHotelAsync(hotelId))
                .ReturnsAsync(orderDatas);

        private void SetupOrderServiceAddAsyncMock(AddOrderModel addOrderModel, DefaultResponseModel expected)
            => orderServiceMock
                .Setup(service => service.AddAsync(addOrderModel))
                .ReturnsAsync(expected);

        private DefaultResponseModel CreateDefaultResponseModel()
           => new DefaultResponseModel { IsSuccessful = true, Message = "Success" };

        private AddOrderModel CreateAddOrderModel()
            => new AddOrderModel();

        private IReadOnlyCollection<GetOrderModel> CreateEmptyOrderDataCollection()
         => new List<GetOrderModel>();
    }
}