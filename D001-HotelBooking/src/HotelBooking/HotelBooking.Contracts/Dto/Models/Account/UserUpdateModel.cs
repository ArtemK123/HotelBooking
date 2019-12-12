namespace HotelBooking.Contracts.Dto.Models.Account
{
    public class UserUpdateModel
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public int RoleId { get; set; }
    }
}