using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models;
using HotelBooking.Contracts.Dto.Models.Orders;
using HotelBooking.Services;
using HotelBooking.Services.Interfaces.Handlers;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.Service
{
    [TestFixture]
    internal class OrderServiceTest : UnitTestBase
    {
        private const string GetAsyncMethodName = nameof(OrderService.GetAsync) + ". ";
        private const string GetByClientAsyncMethodName = nameof(OrderService.GetByClientAsync) + ". ";
        private const string GetByHotelAsyncMethodName = nameof(OrderService.GetByHotelAsync) + ". ";
        private const string AddOrderAsyncMethodName = nameof(OrderService.AddAsync) + ". ";
        private const int ClientId = 1;
        private const int HotelId = 1;
        private const string Token = "token";
        private const string Name = "Name";
        private const string Surname = "Surname";
        private const int RoomId = 1;
        private const bool DatesAreValid = true;
        private const bool DatesAreNotValid = false;
        private const bool RoomIsAvailable = true;
        private const bool RoomIsNotAvailable = false;
        private DateTime checkIn = DateTime.UtcNow;
        private DateTime checkOut = DateTime.UtcNow;

        private Mock<IOrderRepository> orderRepositoryMock;
        private Mock<IClientRepository> clientRepositoryMock;
        private Mock<IHotelRepository> hotelRepositoryMock;
        private Mock<ISessionRepository> sessionRepositoryMock;
        private Mock<IApplicationUserRepository> applicationUserRepositoryMock;
        private Mock<IRoomAvailabilityHandler> roomAvailabilityHandlerMock;

        private OrderService orderService;

        [SetUp]
        public void TestInitialize()
        {
            orderRepositoryMock = MockRepository.Create<IOrderRepository>();
            clientRepositoryMock = MockRepository.Create<IClientRepository>();
            hotelRepositoryMock = MockRepository.Create<IHotelRepository>();
            sessionRepositoryMock = MockRepository.Create<ISessionRepository>();
            applicationUserRepositoryMock = MockRepository.Create<IApplicationUserRepository>();
            roomAvailabilityHandlerMock = MockRepository.Create<IRoomAvailabilityHandler>();

            orderService = new OrderService(
                orderRepositoryMock.Object,
                clientRepositoryMock.Object,
                hotelRepositoryMock.Object,
                sessionRepositoryMock.Object,
                applicationUserRepositoryMock.Object,
                roomAvailabilityHandlerMock.Object);
        }

        [TestCase(TestName = GetAsyncMethodName + "Should return result got from orderRepository GetAsync method")]
        public async Task GetAsyncTest()
        {
            IReadOnlyCollection<GetOrderModel> expected = CreateGetOrderModels();
            SetupOrderRepositoryGetAsyncMock(expected);

            IReadOnlyCollection<GetOrderModel> actual = await orderService.GetAsync();

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetAsyncMethodName +
                             "With id argument should return result got from orderRepository GetAsync(id) method")]
        public async Task GetAsyncByIdTest()
        {
            int orderId = 1;
            GetOrderModel expected = CreateGetOrderModel();

            SetupOrderRepositoryGetAsyncMock(orderId, expected);

            GetOrderModel actual = await orderService.GetAsync(orderId);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetByClientAsyncMethodName +
                             @"Should return result got from orderRepository GetByClientAsync(id)
                                method when client exists")]
        public async Task GetByClientAsyncTest()
        {
            ClientData clientData = CreateClientData();
            IReadOnlyCollection<GetOrderModel> expected = CreateGetOrderModels();

            SetupClientRepositoryMock(clientData, ClientId);
            SetupOrderRepositoryGetByClientAsyncMock(expected, ClientId);

            IReadOnlyCollection<GetOrderModel> actual = await orderService.GetByClientAsync(ClientId);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetByClientAsyncMethodName +
                             "Should return new list of GetOrderModels when client does not exists")]
        public async Task GetByClientAsyncClientDoesNotExistTest()
        {
            IReadOnlyCollection<GetOrderModel> expected = CreateGetOrderModels();

            SetupClientRepositoryMock(null, ClientId);

            IReadOnlyCollection<GetOrderModel> actual = await orderService.GetByClientAsync(ClientId);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetByHotelAsyncMethodName +
                             @"Should return result got from orderRepository GetByHotelAsync(id) 
                                method when hotel exists")]
        public async Task GetByHotelAsyncTest()
        {
            HotelData hotelData = CreateHotelData();
            IReadOnlyCollection<GetOrderModel> expected = CreateGetOrderModels();

            SetupHotelRepositoryMock(hotelData, HotelId);
            SetupOrderRepositoryGetByHotelAsyncMock(expected, HotelId);

            IReadOnlyCollection<GetOrderModel> actual = await orderService.GetByHotelAsync(HotelId);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = GetByHotelAsyncMethodName +
                             "Should return new list of GetOrderModels when hotel does not exists")]
        public async Task GetByHotelAsyncHotelDoesNotExistTest()
        {
            IReadOnlyCollection<GetOrderModel> expected = CreateGetOrderModels();

            SetupHotelRepositoryMock(null, HotelId);
            SetupOrderRepositoryGetByHotelAsyncMock(expected, HotelId);

            IReadOnlyCollection<GetOrderModel> actual = await orderService.GetByHotelAsync(HotelId);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(TestName = AddOrderAsyncMethodName +
                             "Should return true when session is not null and addedOrder is valid")]
        public async Task AddOrderAsyncValidDataTest()
        {
            AddOrderModel addOrderModel = CreateAddOrderModel(Token);
            SessionData sessionData = CreateSessionData();
            UserData userData = CreateUserData();
            ClientData clientData = CreateClientData();

            SetupSessionRepositoryMock(addOrderModel.Token, sessionData);
            SetupApplicationUserRepositoryMock(sessionData.UserId, userData);
            SetupClientRepositoryGetByUserMock(clientData, userData);
            SetupOrderRepositoryAddAsyncMock(addOrderModel, clientData.Id);
            SetupRoomAvailabilityHandlerMock(addOrderModel, DatesAreValid, RoomIsAvailable);

            DefaultResponseModel expected = CreateSuccessfulDefaultResponseModel();
            var actual = await orderService.AddAsync(addOrderModel);

            Assert.AreEqual(expected.IsSuccessful, actual.IsSuccessful);
        }

        [TestCase(TestName = AddOrderAsyncMethodName + "Should return false when session is null")]
        public async Task AddOrderAsyncNullSessionTest()
        {
            AddOrderModel addOrderModel = CreateAddOrderModel(Token);

            SetupSessionRepositoryMock(addOrderModel.Token, null);

            DefaultResponseModel expected = CreateDeniedDefaultResponseModel();
            DefaultResponseModel actual = await orderService.AddAsync(addOrderModel);

            Assert.AreEqual(expected.IsSuccessful, actual.IsSuccessful);
        }

        [TestCase(TestName = AddOrderAsyncMethodName + "Should return false when dates of order are not valid")]
        public async Task AddOrderAsyncInvalidDatesTest()
        {
            AddOrderModel addOrderModel = CreateAddOrderModel(Token);
            SessionData sessionData = CreateSessionData();

            SetupSessionRepositoryMock(addOrderModel.Token, sessionData);
            SetupRoomAvailabilityHandlerAreBookingDatesValidMock(addOrderModel, DatesAreNotValid);

            DefaultResponseModel expected = CreateDeniedDefaultResponseModel();
            DefaultResponseModel actual = await orderService.AddAsync(addOrderModel);

            Assert.AreEqual(expected.IsSuccessful, actual.IsSuccessful);
        }

        [TestCase(TestName = AddOrderAsyncMethodName + "Should return false when room is not available")]
        public async Task AddOrderAsyncRoomIsNotAvailableTest()
        {
            AddOrderModel addOrderModel = CreateAddOrderModel(Token);
            SessionData sessionData = CreateSessionData();

            SetupSessionRepositoryMock(addOrderModel.Token, sessionData);
            SetupRoomAvailabilityHandlerMock(addOrderModel, DatesAreValid, RoomIsNotAvailable);

            DefaultResponseModel expected = CreateDeniedDefaultResponseModel();
            DefaultResponseModel actual = await orderService.AddAsync(addOrderModel);

            Assert.AreEqual(expected.IsSuccessful, actual.IsSuccessful);
        }

        private void SetupOrderRepositoryGetAsyncMock(IReadOnlyCollection<GetOrderModel> getOrderModels)
            => orderRepositoryMock
                .Setup(repository => repository.GetAsync())
                .ReturnsAsync(getOrderModels);

        private void SetupOrderRepositoryGetAsyncMock(int id, GetOrderModel getOrderModel)
            => orderRepositoryMock
                .Setup(repository => repository.GetAsync(id))
                .ReturnsAsync(getOrderModel);

        private void SetupClientRepositoryGetByUserMock(ClientData clientData, UserData userData)
            => clientRepositoryMock
                .Setup(repository => repository.FindByUser(userData))
                .Returns(clientData);

        private void SetupClientRepositoryMock(ClientData clientData, int clientId)
            => clientRepositoryMock
                .Setup(repository => repository.FindByIdAsync(clientId))
                .ReturnsAsync(clientData);

        private void SetupOrderRepositoryGetByClientAsyncMock(
            IReadOnlyCollection<GetOrderModel> getOrderModels,
            int clientId)
                => orderRepositoryMock
                    .Setup(repository => repository.GetByClientAsync(clientId))
                    .ReturnsAsync(getOrderModels);

        private void SetupOrderRepositoryGetByHotelAsyncMock(
            IReadOnlyCollection<GetOrderModel> getOrderModels,
            int clientId)
                => orderRepositoryMock
                    .Setup(repository => repository.GetByHotelAsync(clientId))
                    .ReturnsAsync(getOrderModels);

        private void SetupHotelRepositoryMock(HotelData hotelData, int hotelId)
            => hotelRepositoryMock
                .Setup(repository => repository.GetAsync(hotelId))
                .ReturnsAsync(hotelData);

        private void SetupSessionRepositoryMock(string token, SessionData sessionData)
            => sessionRepositoryMock
                .Setup(repository => repository.GetByTokenAsync(token))
                .ReturnsAsync(sessionData);

        private void SetupApplicationUserRepositoryMock(int userId, UserData userData)
            => applicationUserRepositoryMock
                .Setup(repository => repository.FindByIdAsync(userId))
                .ReturnsAsync(userData);

        private void SetupOrderRepositoryAddAsyncMock(AddOrderModel addOrderModel, int clientId)
            => orderRepositoryMock
                .Setup(repository =>
                    repository.AddAsync(It.Is<OrderData>(order =>
                        HasProperties(
                            order,
                            addOrderModel,
                            clientId))))
                .ReturnsAsync((OrderData)null);

        private void SetupRoomAvailabilityHandlerAreBookingDatesValidMock(AddOrderModel addOrderModel, bool valid)
            => roomAvailabilityHandlerMock
                .Setup(handler => handler.AreBookingDatesValid(addOrderModel))
                .ReturnsAsync(valid);

        private void SetupRoomAvailabilityHandlerIsRoomAvailableAsyncMock(AddOrderModel addOrderModel, bool valid)
            => roomAvailabilityHandlerMock
                .Setup(handler => handler.IsRoomAvailableAsync(addOrderModel))
                .ReturnsAsync(valid);

        private void SetupRoomAvailabilityHandlerMock(AddOrderModel addOrderModel, bool areDatesValid, bool isAvailable)
        {
            SetupRoomAvailabilityHandlerAreBookingDatesValidMock(addOrderModel, areDatesValid);
            SetupRoomAvailabilityHandlerIsRoomAvailableAsyncMock(addOrderModel, isAvailable);
        }

        private IReadOnlyCollection<GetOrderModel> CreateGetOrderModels()
            => new List<GetOrderModel>();

        private GetOrderModel CreateGetOrderModel() => new GetOrderModel();

        private ClientData CreateClientData() => new ClientData();

        private HotelData CreateHotelData() => new HotelData();

        private AddOrderModel CreateAddOrderModel(string token) =>
            new AddOrderModel()
            {
                Token = token,
                Name = Name,
                Surname = Surname,
                RoomId = RoomId,
                CheckIn = checkIn,
                CheckOut = checkOut
            };

        private SessionData CreateSessionData() => new SessionData();

        private UserData CreateUserData() => new UserData();

        private DefaultResponseModel CreateSuccessfulDefaultResponseModel()
            => new DefaultResponseModel { IsSuccessful = true, Message = "Success" };

        private DefaultResponseModel CreateDeniedDefaultResponseModel()
           => new DefaultResponseModel { IsSuccessful = false, Message = "Denied" };

        private bool HasProperties(OrderData order, AddOrderModel addOrderModel, int clientId) =>
            order.Name == addOrderModel.Name &&
            order.Surname == addOrderModel.Surname &&
            DateTime.Compare(order.CheckIn, addOrderModel.CheckIn) == 0 &&
            DateTime.Compare(order.CheckOut, addOrderModel.CheckOut) == 0 &&
            order.RoomId == addOrderModel.RoomId &&
            order.ClientId == clientId;
    }
}