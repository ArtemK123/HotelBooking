using System.Collections.Generic;
using System.Linq;
using HotelBooking.Contracts.Services.PipelinePattern.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern.Filters
{
    internal class HotelCityFilter : IFilter<IReadOnlyCollection<Hotel>>
    {
        private readonly string cityName;

        public HotelCityFilter(string cityName)
        {
            this.cityName = cityName;
        }

        public IReadOnlyCollection<Hotel> Execute(IReadOnlyCollection<Hotel> input) => string.IsNullOrEmpty(cityName)
            ? input.ToList()
            : input.Where(hotel => hotel.City.ToLower().Contains(cityName.ToLower())).ToList();
    }
}