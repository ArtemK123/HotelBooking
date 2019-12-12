namespace HotelBooking.Contracts.Dto.Models.Register
{
    public class RegistrationResponseModel
    {
        public bool IsSuccessful { get; set; }

        public string Token { get; set; }

        public string Message { get; set; }
    }
}