using System.Collections.Generic;

namespace HotelBooking.Contracts.Dto.Data
{
    public class HotelData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IReadOnlyCollection<string> Images { get; set; }

        public int Rating { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int ManagerId { get; set; }
    }
}