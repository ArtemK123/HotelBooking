using System.Collections.Generic;
using System.Linq;
using HotelBooking.Contracts.Services.PipelinePattern.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern.Filters
{
    internal class RoomPriceFromFilter : IFilter<IReadOnlyCollection<Room>>
    {
        private readonly int? priceFrom;

        public RoomPriceFromFilter(int? priceFrom)
        {
            this.priceFrom = priceFrom;
        }

        public IReadOnlyCollection<Room> Execute(IReadOnlyCollection<Room> input) => priceFrom == null
            ? input
            : input.Where(room => room.PricePerNight >= priceFrom).ToList();
    }
}