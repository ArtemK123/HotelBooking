using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    internal class RoomImage
    {
        public int Id { get; set; }

        public string Path { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
    }
}