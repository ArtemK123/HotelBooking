using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Search;
using HotelBooking.Contracts.Services;

namespace HotelBooking.Services
{
    internal class HotelService : IHotelService
    {
        private const int DefaultPageSize = 20;
        private readonly IHotelRepository hotelRepository;

        public HotelService(IHotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public async Task<IReadOnlyCollection<HotelData>> GetAsync() => await hotelRepository.GetAsync();

        public int CountSearchResults(DataForSearch dataForSearch) => hotelRepository.CountSearchResults(dataForSearch);

        public IReadOnlyCollection<HotelData> GetSearchResultByPage(DataForSearch dataForSearch) =>
            dataForSearch == null
                ? hotelRepository.GetByPage(1, DefaultPageSize)
                : hotelRepository.GetSearchResultByPage(dataForSearch, DefaultPageSize);

        public async Task<HotelData> GetAsync(int id) => await hotelRepository.GetAsync(id);
    }
}