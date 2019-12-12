using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Contracts.Services.PipelinePattern.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern.Filters
{
    internal class HotelNameFilter : IFilter<IReadOnlyCollection<Hotel>>
    {
        private readonly string substring;

        public HotelNameFilter(string substring)
        {
            this.substring = substring;
        }

        public IReadOnlyCollection<Hotel> Execute(IReadOnlyCollection<Hotel> input) => string.IsNullOrEmpty(substring)
            ? input
            : input.Where(hotel => hotel.Name.ToLower().Contains(substring.ToLower())).ToList();
    }
}