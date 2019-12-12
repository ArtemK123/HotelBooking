using System.Collections.Generic;
using HotelBooking.Contracts.Services.PipelinePattern.Interfaces;

namespace HotelBooking.Contracts.Services.PipelinePattern.AbstractClasses
{
    public abstract class Pipeline<T>
    {
        protected readonly List<IFilter<T>> filters = new List<IFilter<T>>();

        public Pipeline<T> Register(IFilter<T> filter)
        {
            filters.Add(filter);
            return this;
        }

        public abstract T Process(T input);
    }
}