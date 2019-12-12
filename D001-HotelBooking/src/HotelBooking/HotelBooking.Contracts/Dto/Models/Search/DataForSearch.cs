namespace HotelBooking.Contracts.Dto.Models.Search
{
    public class DataForSearch
    {
        public HotelRequirements HotelRequirements { get; set; }

        public RoomRequirements RoomRequirements { get; set; }

        public int Page { get; set; }
    }
}