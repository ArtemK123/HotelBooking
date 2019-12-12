using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Domain.PipelinePattern;
using HotelBooking.Domain.PipelinePattern.Filters;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repositories
{
    internal class RoomRepository : IRoomRepository
    {
        private readonly IApplicationDbContext context;

        public RoomRepository(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyCollection<RoomData>> GetAsync() => Map(await context.Rooms.ToListAsync());

        public async Task<RoomData> GetAsync(int id)
        {
            var room = await context.Rooms.FindAsync(id);
            return room == null ? null : Map(room);
        }

        public async Task<IReadOnlyCollection<RoomData>> GetByHotelIdAsync(int hotelId)
        {
            var rooms = await context.Rooms.Where(room => room.HotelId == hotelId).ToListAsync();
            return rooms == null ? null : Map(rooms);
        }

        public IReadOnlyCollection<RoomData> SearchByFilter(RoomRequirements roomRequirements)
        {
            var roomsSelectionPipeline = new RoomsSelectionPipeline();
            roomsSelectionPipeline.Register(new RoomPriceFromFilter(roomRequirements.PriceFrom))
                .Register(new RoomPriceToFilter(roomRequirements.PriceTo))
                .Register(new RoomCapacityFilter(roomRequirements.RequiredCapacity))
                .Register(new RoomAvailabilityFilter(roomRequirements.CheckIn, roomRequirements.CheckOut, context.Orders.ToList()));
            return Map(roomsSelectionPipeline.Process(context.Rooms.Where(room => room.HotelId == roomRequirements.HotelId)
                .ToList()));
        }

        private IReadOnlyCollection<RoomData> Map(IReadOnlyCollection<Room> rooms)
            => rooms.Select(Map).ToList();

        private RoomData Map(Room room)
            => new RoomData
            {
                Description = room.Description,
                HotelId = room.HotelId,
                NumberOfPeople = room.NumberOfPeople,
                PricePerNight = room.PricePerNight,
                Id = room.Id,
                Square = room.Square,
                Images = Map(context.RoomImages.Where(image => image.RoomId == room.Id).ToList()),
            };

        private IReadOnlyCollection<string> Map(IReadOnlyCollection<RoomImage> roomImages)
            => roomImages.Select(image => image.Path).ToList();
    }
}