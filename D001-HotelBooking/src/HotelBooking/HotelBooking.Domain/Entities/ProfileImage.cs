using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    internal class ProfileImage
    {
        public int Id { get; set; }

        public string Path { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
    }
}