namespace HotelBooking.Contracts.Dto.Data
{
    public class CreditCardData
    {
        public int Id { get; set; }

        public long Number { get; set; }

        public string DueDate { get; set; }

        public int CV2 { get; set; }

        public int ClientId { get; set; }
    }
}