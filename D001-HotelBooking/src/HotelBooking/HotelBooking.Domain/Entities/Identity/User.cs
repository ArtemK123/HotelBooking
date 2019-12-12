using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Domain.Entities.Identity
{
    internal class User : IdentityUser<int>
    {
        [ForeignKey("IdentityRole<int>")]
        public int RoleId { get; set; }
    }
}