using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;

namespace HotelBooking.Contracts.Domain.Repositories
{
    public interface IHotelRepository
    {
        Task<IReadOnlyCollection<HotelData>> GetAsync();

        Task<HotelData> GetAsync(int id);

        IReadOnlyCollection<HotelData> GetByPage(int page, int pageSize);

        IReadOnlyCollection<HotelData> GetSearchResultByPage(DataForSearch dataForSearchData, int pageSize);

        int CountSearchResults(DataForSearch dataForSearch);
    }
}