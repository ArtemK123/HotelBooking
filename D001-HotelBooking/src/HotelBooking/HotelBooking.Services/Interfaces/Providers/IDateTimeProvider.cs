using System;

namespace HotelBooking.Services.Interfaces.Providers
{
    internal interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}