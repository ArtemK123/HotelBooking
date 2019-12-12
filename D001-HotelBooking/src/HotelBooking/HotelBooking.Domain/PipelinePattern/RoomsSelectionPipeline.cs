using System.Collections.Generic;
using HotelBooking.Contracts.Services.PipelinePattern.AbstractClasses;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern
{
    internal class RoomsSelectionPipeline : Pipeline<IReadOnlyCollection<Room>>
    {
        public override IReadOnlyCollection<Room> Process(IReadOnlyCollection<Room> input)
        {
            foreach (var filter in filters)
            {
                input = filter.Execute(input);
            }

            return input;
        }
    }
}