using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Enums;
using HotelBooking.Contracts.Dto.Models.Orders;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly IApplicationDbContext context;

        public OrderRepository(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<GetOrderModel>> GetAsync() => GetMap(await context.Orders.ToListAsync());

        public async Task<GetOrderModel> GetAsync(int id) => GetMap(await context.Orders.FindAsync(id));

        public async Task<IReadOnlyCollection<GetOrderModel>> GetByHotelAsync(int id) => GetMap(
            await context.Orders
                .Where(order =>
                    context.Rooms.Find(order.RoomId)
                        .HotelId == id)
                .ToListAsync());

        public async Task<IReadOnlyCollection<GetOrderModel>> GetByClientAsync(int id) => GetMap(
            await context.Orders.Where(order => order.ClientId == id).ToListAsync());

        public async Task<OrderData> AddAsync(OrderData orderData)
        {
            var addingResult = await context.Orders.AddAsync(Map(orderData));
            context.SaveChanges();
            return Map(addingResult.Entity);
        }

        private GetOrderModel GetMap(Order order)
        {
            var numberOfDays = order.CheckOut.Subtract(order.CheckIn).Days;
            int hotelId = context.Rooms.Find(order.RoomId).HotelId;
            return new GetOrderModel()
            {
                HotelId = hotelId,
                HotelName = context.Hotels.Find(hotelId).Name,
                CheckIn = order.CheckIn.ToShortDateString(),
                CheckOut = order.CheckOut.Date.ToShortDateString(),
                NumberOfDays = numberOfDays,
                PersonName = order.Name,
                PersonSurname = order.Surname,
                TotalPrice = numberOfDays * context.Rooms.Find(order.RoomId).PricePerNight,
                State = (DateTime.Compare(DateTime.UtcNow, order.CheckOut) > 0) ?
                    OrderState.Passed :
                    (DateTime.Compare(DateTime.UtcNow, order.CheckIn) < 0 ? OrderState.Future : OrderState.Active),
            };
        }

        private IReadOnlyCollection<GetOrderModel> GetMap(IReadOnlyCollection<Order> orders) => orders.Select(GetMap).ToList();

        private Order Map(OrderData order)
            => new Order
            {
                ClientId = order.ClientId,
                RoomId = order.RoomId,
                CheckIn = order.CheckIn,
                CheckOut = order.CheckOut,
                Name = order.Name,
                Surname = order.Surname,
            };

        private OrderData Map(Order order)
            => new OrderData
            {
                Id = order.Id,
                ClientId = order.ClientId,
                RoomId = order.RoomId,
                CheckIn = order.CheckIn,
                CheckOut = order.CheckOut,
                Name = order.Name,
                Surname = order.Surname,
            };
    }
}