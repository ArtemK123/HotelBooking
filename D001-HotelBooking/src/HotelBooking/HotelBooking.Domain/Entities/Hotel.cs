using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    internal class Hotel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Rating { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
    }
}