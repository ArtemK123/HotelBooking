using System.IdentityModel.Tokens.Jwt;
using HotelBooking.Services.Factories;
using HotelBooking.Services.Interfaces.Factories;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.Service.Factories
{
    [TestFixture]
    internal class JavascriptWebTokenFactoryTest : UnitTestBase
    {
        private const string CreateMethodName = nameof(JavascriptWebTokenFactory.Create) + ". ";
        private const int UserId = 3;
        private const string Token = "Token";

        private JavascriptWebTokenFactory javascriptWebTokenFactory;

        private Mock<ISecurityTokenDescriptorFactory> securityTokenDescriptorFactoryMock;
        private Mock<JwtSecurityTokenHandler> jwtSecurityTokenHandlerMock;
        private Mock<SecurityToken> securityTokenMock;

        [SetUp]
        public void TestInitialize()
        {
            securityTokenDescriptorFactoryMock = MockRepository.Create<ISecurityTokenDescriptorFactory>();
            jwtSecurityTokenHandlerMock = MockRepository.Create<JwtSecurityTokenHandler>();
            securityTokenMock = MockRepository.Create<SecurityToken>();
            javascriptWebTokenFactory = new JavascriptWebTokenFactory(securityTokenDescriptorFactoryMock.Object, jwtSecurityTokenHandlerMock.Object);
        }

        [TestCase(TestName = CreateMethodName + "Should return valid string token")]
        public void GetClientTest()
        {
            SecurityTokenDescriptor securityTokenDescriptor = CreateSecurityTokenDescriptor();
            SetupSecurityTokenDescriptorFactoryMock(UserId, securityTokenDescriptor);
            SetupJwtSecurityTokenHandlerMock(securityTokenMock.Object, securityTokenDescriptor);

            string actual = javascriptWebTokenFactory.Create(UserId);

            Assert.AreEqual(actual, Token);
        }

        private void SetupSecurityTokenDescriptorFactoryMock(int userId, SecurityTokenDescriptor securityTokenDescriptor)
           => securityTokenDescriptorFactoryMock
               .Setup(factory => factory.Create(userId))
               .Returns(securityTokenDescriptor);

        private void SetupJwtSecurityTokenHandlerMock(SecurityToken securityToken, SecurityTokenDescriptor securityTokenDescriptor)
        {
            jwtSecurityTokenHandlerMock
               .Setup(tokenHandler => tokenHandler.CreateToken(securityTokenDescriptor))
               .Returns(securityToken);

            jwtSecurityTokenHandlerMock
               .Setup(tokenHandler => tokenHandler.WriteToken(securityToken))
               .Returns(Token);
        }

        private SecurityTokenDescriptor CreateSecurityTokenDescriptor()
            => new SecurityTokenDescriptor();
    }
}