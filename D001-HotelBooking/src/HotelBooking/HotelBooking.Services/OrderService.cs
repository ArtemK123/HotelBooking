using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models;
using HotelBooking.Contracts.Dto.Models.Orders;
using HotelBooking.Contracts.Services;
using HotelBooking.Services.Interfaces.Handlers;

namespace HotelBooking.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IClientRepository clientRepository;
        private readonly IHotelRepository hotelRepository;
        private readonly ISessionRepository sessionRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IRoomAvailabilityHandler roomAvailabilityHandler;

        public OrderService(
            IOrderRepository orderRepository,
            IClientRepository clientRepository,
            IHotelRepository hotelRepository,
            ISessionRepository sessionRepository,
            IApplicationUserRepository applicationUserRepository,
            IRoomAvailabilityHandler roomAvailabilityHandler)
        {
            this.orderRepository = orderRepository;
            this.clientRepository = clientRepository;
            this.hotelRepository = hotelRepository;
            this.sessionRepository = sessionRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.roomAvailabilityHandler = roomAvailabilityHandler;
        }

        public async Task<IReadOnlyCollection<GetOrderModel>> GetAsync() => await orderRepository.GetAsync();

        public async Task<GetOrderModel> GetAsync(int id) => await orderRepository.GetAsync(id);

        public async Task<IReadOnlyCollection<GetOrderModel>> GetByClientAsync(int id) =>
            (await clientRepository.FindByIdAsync(id) != null) ?
                await orderRepository.GetByClientAsync(id) :
                new List<GetOrderModel>();

        public async Task<IReadOnlyCollection<GetOrderModel>> GetByHotelAsync(int hotelId) =>
            (hotelRepository.GetAsync(hotelId) != null) ?
                await orderRepository.GetByHotelAsync(hotelId) :
                new List<GetOrderModel>();

        public async Task<DefaultResponseModel> AddAsync(AddOrderModel addOrderModel)
        {
            DefaultResponseModel response = new DefaultResponseModel { IsSuccessful = false, Message = string.Empty };
            SessionData session = await sessionRepository.GetByTokenAsync(addOrderModel.Token);
            if (session == null)
            {
                response.Message = "Unathorized user";
                return response;
            }
            if (!await roomAvailabilityHandler.AreBookingDatesValid(addOrderModel))
            {
                response.Message = "Dates are not valid";
                return response;
            }
            if (!await roomAvailabilityHandler.IsRoomAvailableAsync(addOrderModel))
            {
                response.Message = "Room already booked for this dates";
                return response;
            }

            UserData user = await applicationUserRepository.FindByIdAsync(session.UserId);
            ClientData client = clientRepository.FindByUser(user);
            var orderData = new OrderData
            {
                Name = addOrderModel.Name,
                Surname = addOrderModel.Surname,
                RoomId = addOrderModel.RoomId,
                CheckIn = addOrderModel.CheckIn,
                CheckOut = addOrderModel.CheckOut,
                ClientId = client.Id,
            };
            OrderData addedOrder = await orderRepository.AddAsync(orderData);

            response.IsSuccessful = true;
            return response;
        }
    }
}