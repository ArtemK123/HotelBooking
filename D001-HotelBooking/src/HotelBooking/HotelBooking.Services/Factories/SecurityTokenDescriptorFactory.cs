using System.Security.Claims;
using System.Text;
using HotelBooking.Services.Interfaces.Factories;
using HotelBooking.Services.Interfaces.Providers;
using Microsoft.IdentityModel.Tokens;

namespace HotelBooking.Services.Factories
{
    internal class SecurityTokenDescriptorFactory : ISecurityTokenDescriptorFactory
    {
        private const string SecurityKey = "1234567890123456";
        private const string UserIdClaimName = "UserId";

        private readonly IDateTimeProvider dateTimeProvider;

        public SecurityTokenDescriptorFactory(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public SecurityTokenDescriptor Create(int userId)
            => new SecurityTokenDescriptor
            {
                Subject = CreateClaimsIdentity(userId),
                Expires = dateTimeProvider.UtcNow.AddDays(1),
                SigningCredentials = CreateSigningCredentials()
            };

        private ClaimsIdentity CreateClaimsIdentity(int userId)
            => new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(UserIdClaimName, userId.ToString()),
                });

        private SigningCredentials CreateSigningCredentials()
            => new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey)),
                SecurityAlgorithms.HmacSha256Signature);
    }
}