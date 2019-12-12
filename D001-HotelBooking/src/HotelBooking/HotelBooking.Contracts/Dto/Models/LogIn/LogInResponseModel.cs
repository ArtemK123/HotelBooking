namespace HotelBooking.Contracts.Dto.Models.LogIn
{
    public class LogInResponseModel
    {
        public bool IsSuccessful { get; set; }

        public string Token { get; set; }

        public string Message { get; set; }
    }
}