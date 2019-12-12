namespace HotelBooking.Contracts.Dto.Models.Register
{
    public class RegistrationRequestModel
    {
        public string UserName { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}