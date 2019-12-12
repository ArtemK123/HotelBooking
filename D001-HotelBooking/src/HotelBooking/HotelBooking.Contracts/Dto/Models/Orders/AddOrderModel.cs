using System;

namespace HotelBooking.Contracts.Dto.Models.Orders
{
    public class AddOrderModel
    {
        public string Token { get; set; }

        public int RoomId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public long CreditCardNumber { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }
    }
}