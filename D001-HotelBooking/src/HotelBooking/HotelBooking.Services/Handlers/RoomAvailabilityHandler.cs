using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Models.Orders;
using HotelBooking.Services.Interfaces.Handlers;

namespace HotelBooking.Services.Handlers
{
    internal class RoomAvailabilityHandler : IRoomAvailabilityHandler
    {
        private readonly IOrderRepository orderRepository;

        public RoomAvailabilityHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<bool> AreBookingDatesValid(AddOrderModel addOrderModel)
            => DateTime.Compare(addOrderModel.CheckOut, addOrderModel.CheckIn) > 0 &&
               DateTime.Compare(addOrderModel.CheckIn, DateTime.UtcNow) > 0;

        public async Task<bool> IsRoomAvailableAsync(AddOrderModel addOrderModel)
        {
            DateTime checkIn = addOrderModel.CheckIn;
            DateTime checkOut = addOrderModel.CheckOut;

            IReadOnlyCollection <GetOrderModel> orders = await orderRepository.GetAsync();
            foreach (var order in orders)
            {
                if (IsDatesOverlay(checkIn, checkOut, DateTime.Parse(order.CheckIn), DateTime.Parse(order.CheckOut)))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDatesOverlay(DateTime checkIn1, DateTime checkOut1, DateTime checkIn2, DateTime checkOut2)
        {
            return DateTime.Compare(checkIn1, checkOut2) < 0 && DateTime.Compare(checkOut1, checkIn2) > 0;
        }
    }
}