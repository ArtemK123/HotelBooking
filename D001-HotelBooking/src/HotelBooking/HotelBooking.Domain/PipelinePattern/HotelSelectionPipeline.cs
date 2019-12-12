using System.Collections.Generic;
using HotelBooking.Contracts.Services.PipelinePattern.AbstractClasses;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.PipelinePattern
{
    internal class HotelSelectionPipeline : Pipeline<IReadOnlyCollection<Hotel>>
    {
        public override IReadOnlyCollection<Hotel> Process(IReadOnlyCollection<Hotel> input)
        {
            foreach (var filter in filters)
            {
                input = filter.Execute(input);
            }

            return input;
        }
    }
}