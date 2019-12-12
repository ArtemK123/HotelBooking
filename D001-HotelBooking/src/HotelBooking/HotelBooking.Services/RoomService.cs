using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;
using HotelBooking.Contracts.Services;

namespace HotelBooking.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public async Task<IReadOnlyCollection<RoomData>> GetAsync() => await roomRepository.GetAsync();

        public async Task<RoomData> GetAsync(int id) => await roomRepository.GetAsync(id);

        public IReadOnlyCollection<RoomData> SearchByFilters(RoomRequirements roomRequirements)
        {
            return roomRepository.SearchByFilter(roomRequirements);
        }

        public async Task<IReadOnlyCollection<RoomData>> GetByHotelIdAsync(int hotelId) => await roomRepository.GetByHotelIdAsync(hotelId);
    }
}