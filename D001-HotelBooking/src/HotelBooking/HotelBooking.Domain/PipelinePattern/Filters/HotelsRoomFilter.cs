using System.Collections.Generic;
using System.Linq;
using HotelBooking.Contracts.Dto.Models.Search;
using HotelBooking.Contracts.Services.PipelinePattern.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern.Filters
{
    internal class HotelsRoomFilter : IFilter<IReadOnlyCollection<Hotel>>
    {
        private readonly RoomRequirements roomRequirements;
        private readonly IEnumerable<Room> rooms;
        private readonly IEnumerable<Order> orders;

        public HotelsRoomFilter(RoomRequirements roomRequirements, IReadOnlyCollection<Room> rooms, IReadOnlyCollection<Order> orders)
        {
            this.roomRequirements = roomRequirements;
            this.rooms = rooms;
            this.orders = orders;
        }

        public IReadOnlyCollection<Hotel> Execute(IReadOnlyCollection<Hotel> input)
        {
            if (!roomRequirements.Exists())
            {
                return input;
            }
            var filteredHotels = new List<Hotel>();
            foreach (var hotel in input)
            {
                var roomsSelectionPipeline = new RoomsSelectionPipeline();
                var roomsOfHotel = rooms.Where(room => room.HotelId == hotel.Id).ToList();
                roomsSelectionPipeline
                    .Register(new RoomPriceFromFilter(roomRequirements.PriceFrom))
                    .Register(new RoomPriceToFilter(roomRequirements.PriceTo))
                    .Register(new RoomCapacityFilter(roomRequirements.RequiredCapacity))
                    .Register(
                        new RoomAvailabilityFilter(roomRequirements.CheckIn, roomRequirements.CheckOut, orders.ToList()));
                IEnumerable<Room> filteredRooms = roomsSelectionPipeline.Process(roomsOfHotel);
                if (filteredRooms != null && filteredRooms.Any())
                {
                    filteredHotels.Add(hotel);
                }
            }

            return filteredHotels;
        }
    }
}