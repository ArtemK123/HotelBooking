using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    internal class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Square { get; set; }

        public int NumberOfPeople { get; set; }

        public int PricePerNight { get; set; }

        public string Description { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
    }
}