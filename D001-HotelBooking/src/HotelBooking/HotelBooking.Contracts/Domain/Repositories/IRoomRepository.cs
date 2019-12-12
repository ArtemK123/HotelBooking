using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;

namespace HotelBooking.Contracts.Domain.Repositories
{
    public interface IRoomRepository
    {
        Task<IReadOnlyCollection<RoomData>> GetAsync();

        Task<RoomData> GetAsync(int id);

        Task<IReadOnlyCollection<RoomData>> GetByHotelIdAsync(int hotelId);

        IReadOnlyCollection<RoomData> SearchByFilter(RoomRequirements roomRequirements);
    }
}