using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    internal class CreditCard
    {
        [Key]
        public int Id { get; set; }

        public long Number { get; set; }

        public string DueDate { get; set; }

        public int CV2 { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
    }
}