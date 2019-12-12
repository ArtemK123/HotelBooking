using System.Collections.Generic;
using System.Linq;
using HotelBooking.Contracts.Services.PipelinePattern.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern.Filters
{
    internal class RoomPriceToFilter : IFilter<IReadOnlyCollection<Room>>
    {
        private readonly int? priceTo;

        public RoomPriceToFilter(int? priceTo)
        {
            this.priceTo = priceTo;
        }

        public IReadOnlyCollection<Room> Execute(IReadOnlyCollection<Room> input) => priceTo == null
        ? input
        : input.Where(room => room.PricePerNight <= priceTo).ToList();
    }
}