using System;
using HotelBooking.Services.Interfaces.Providers;

namespace HotelBooking.Services.Providers
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}