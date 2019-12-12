using Microsoft.IdentityModel.Tokens;

namespace HotelBooking.Services.Interfaces.Factories
{
    internal interface ISecurityTokenDescriptorFactory
    {
        SecurityTokenDescriptor Create(int userId);
    }
}