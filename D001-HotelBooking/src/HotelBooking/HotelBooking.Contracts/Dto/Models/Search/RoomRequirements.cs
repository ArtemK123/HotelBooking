using System;

namespace HotelBooking.Contracts.Dto.Models.Search
{
    public class RoomRequirements
    {
        public int? PriceFrom { get; set; }

        public int? PriceTo { get; set; }

        public DateTime? CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public int? RequiredCapacity { get; set; }

        public int? HotelId { get; set; }

        public bool Exists() => PriceFrom != null || PriceTo != null || CheckIn != null || CheckOut != null || RequiredCapacity != null || HotelId != null;
    }
}