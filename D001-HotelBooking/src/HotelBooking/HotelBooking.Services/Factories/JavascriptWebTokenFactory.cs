using System.IdentityModel.Tokens.Jwt;
using HotelBooking.Services.Interfaces.Factories;
using Microsoft.IdentityModel.Tokens;

namespace HotelBooking.Services.Factories
{
    internal class JavascriptWebTokenFactory : IJavascriptWebTokenFactory
    {
        private readonly ISecurityTokenDescriptorFactory securityTokenDescriptorFactory;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public JavascriptWebTokenFactory(ISecurityTokenDescriptorFactory securityTokenDescriptorFactory, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            this.securityTokenDescriptorFactory = securityTokenDescriptorFactory;
            this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public string Create(int userId)
        {
            SecurityTokenDescriptor tokenDescriptor = securityTokenDescriptorFactory.Create(userId);
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}