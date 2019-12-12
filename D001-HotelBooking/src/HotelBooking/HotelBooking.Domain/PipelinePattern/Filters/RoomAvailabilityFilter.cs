using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Contracts.Services.PipelinePattern.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern.Filters
{
    internal class RoomAvailabilityFilter : IFilter<IReadOnlyCollection<Room>>
    {
        private readonly DateTime? checkInNullable;
        private readonly DateTime? checkOutNullable;
        private readonly IEnumerable<Order> orders;

        public RoomAvailabilityFilter(DateTime? checkInNullable, DateTime? checkOutNullable, IReadOnlyCollection<Order> orders)
        {
            this.checkInNullable = checkInNullable;
            this.checkOutNullable = checkOutNullable;
            this.orders = orders;
        }

        public IReadOnlyCollection<Room> Execute(IReadOnlyCollection<Room> input)
        {
            if (checkInNullable == null || checkOutNullable == null)
            {
                return input;
            }

            DateTime checkIn = checkInNullable.Value;
            DateTime checkOut = checkOutNullable.Value;
            var availableRooms = new List<Room>();
            foreach (var room in input)
            {
                var actualOrders = orders.Where(order =>
                       order.RoomId == room.Id && DateTime.Compare(order.CheckOut, DateTime.UtcNow) >= 0);
                if (!actualOrders.Any())
                {
                    availableRooms.Add(room);
                    break;
                }

                if (actualOrders.Any(order =>
             (DateTime.Compare(checkIn, order.CheckIn) < 0 &&
             DateTime.Compare(checkOut, order.CheckIn) < 0) ||
             (DateTime.Compare(order.CheckOut, checkIn) < 0 && DateTime.Compare(order.CheckOut, checkIn) < 0)))
                {
                    availableRooms.Add(room);
                }
            }

            return availableRooms;
        }
    }
}