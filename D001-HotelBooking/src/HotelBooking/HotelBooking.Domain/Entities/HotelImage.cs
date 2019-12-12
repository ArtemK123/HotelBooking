using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    internal class HotelImage
    {
        public int Id { get; set; }

        public string Path { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
    }
}