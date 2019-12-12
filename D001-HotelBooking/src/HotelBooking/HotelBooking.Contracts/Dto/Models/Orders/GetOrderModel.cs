using System;
using HotelBooking.Contracts.Dto.Enums;

namespace HotelBooking.Contracts.Dto.Models.Orders
{
    public class GetOrderModel
    {
        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public string PersonName { get; set; }

        public string PersonSurname { get; set; }

        public string CheckIn { get; set; }

        public string CheckOut { get; set; }

        public int NumberOfDays { get; set; }

        public int TotalPrice { get; set; }

        public OrderState State { get; set; }
    }
}