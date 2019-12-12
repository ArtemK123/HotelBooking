using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities.Users
{
    internal class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Telephone { get; set; }

        public string Passport { get; set; }

        public string PhotoPath { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}