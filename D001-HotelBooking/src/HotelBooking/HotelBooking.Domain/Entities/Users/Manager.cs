using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities.Users
{
    internal class Manager
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}