namespace HotelBooking.Contracts.Dto.Models.Account
{
    public class UpdateClientRequestModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Passport { get; set; }

        public string Telephone { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}