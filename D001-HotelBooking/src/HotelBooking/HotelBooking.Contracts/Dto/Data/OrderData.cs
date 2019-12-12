using System;

namespace HotelBooking.Contracts.Dto.Data
{
    public class OrderData
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int RoomId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }
    }
}