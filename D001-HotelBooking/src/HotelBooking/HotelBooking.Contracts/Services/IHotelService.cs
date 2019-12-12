using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;

namespace HotelBooking.Contracts.Services
{
    public interface IHotelService
    {
        Task<IReadOnlyCollection<HotelData>> GetAsync();

        int CountSearchResults(DataForSearch dataForSearch);

        IReadOnlyCollection<HotelData> GetSearchResultByPage(DataForSearch dataForSearch);

        Task<HotelData> GetAsync(int id);
    }
}