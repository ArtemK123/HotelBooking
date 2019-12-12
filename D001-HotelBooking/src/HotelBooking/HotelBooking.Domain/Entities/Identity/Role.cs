using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Domain.Entities.Identity
{
    internal class Role : IdentityRole<int>
    {
        public Role(string name)
            : base(name)
        {
        }
    }
}