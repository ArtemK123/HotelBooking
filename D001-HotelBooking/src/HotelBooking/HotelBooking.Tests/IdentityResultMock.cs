using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Tests
{
    public class IdentityResultMock : IdentityResult
    {
        public IdentityResultMock(bool succeeded)
            : base()
        {
            Succeeded = succeeded;
        }
    }
}