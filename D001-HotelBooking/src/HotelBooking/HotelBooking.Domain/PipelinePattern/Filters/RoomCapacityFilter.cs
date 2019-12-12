using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelBooking.Contracts.Services.PipelinePattern.Interfaces;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern.Filters
{
    internal class RoomCapacityFilter : IFilter<IReadOnlyCollection<Room>>
    {
        private readonly int? requiredCapacity;

        public RoomCapacityFilter(int? requiredCapacity)
        {
            this.requiredCapacity = requiredCapacity;
        }

        public IReadOnlyCollection<Room> Execute(IReadOnlyCollection<Room> input) => requiredCapacity == null
            ? input
            : input.Where(room => room.NumberOfPeople >= requiredCapacity).ToList();
    }
}