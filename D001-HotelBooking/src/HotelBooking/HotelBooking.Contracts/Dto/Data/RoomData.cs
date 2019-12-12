using System.Collections.Generic;

namespace HotelBooking.Contracts.Dto.Data
{
    public class RoomData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Square { get; set; }

        public int NumberOfPeople { get; set; }

        public int PricePerNight { get; set; }

        public string Description { get; set; }

        public int HotelId { get; set; }

        public IReadOnlyCollection<string> Images { get; set; }
    }
}