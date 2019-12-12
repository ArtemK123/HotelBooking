using System;
using System.Security.Claims;
using System.Text;
using HotelBooking.Services.Factories;
using HotelBooking.Services.Interfaces.Providers;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.Service
{
    [TestFixture]
    internal class SecurityTokenDescriptorFactoryTest : UnitTestBase
    {
        private const string CreateMethodName = nameof(SecurityTokenDescriptorFactory.Create) + ". ";

        private int userId = 1;
        private DateTime dateTime = new DateTime(2020, 12, 12);
        private string securityKey = "1234567890123456";
        private string userIdClaimName = "UserId";

        private SecurityTokenDescriptorFactory securityTokenDescriptorFactory;

        private Mock<IDateTimeProvider> dateTimeProviderMock;

        [SetUp]
        public void TestInitialize()
        {
            dateTimeProviderMock = MockRepository.Create<IDateTimeProvider>();
            securityTokenDescriptorFactory = new SecurityTokenDescriptorFactory(dateTimeProviderMock.Object);
        }

        [TestCase(TestName = CreateMethodName + "Should return security token descriptor")]
        public void CreateTest()
        {
            SecurityTokenDescriptor expected = CreateSecurityTokenDescriptor(userId);

            SetupDateTimeProviderMock(dateTime);

            SecurityTokenDescriptor actual = securityTokenDescriptorFactory.Create(userId);

            Assert.IsTrue(
                actual.Subject.ToString().Equals(expected.Subject.ToString()) &&
                actual.Expires.Equals(expected.Expires));
        }

        private void SetupDateTimeProviderMock(DateTime dateTime)
            => dateTimeProviderMock
                .SetupGet(provider => provider.UtcNow)
                .Returns(dateTime);

        private SecurityTokenDescriptor CreateSecurityTokenDescriptor(int userId)
            => new SecurityTokenDescriptor
            {
                Subject = CreateClaimsIdentity(userId),
                Expires = dateTime.AddDays(1),
                SigningCredentials = CreateSigningCredentials()
            };

        private ClaimsIdentity CreateClaimsIdentity(int userId)
            => new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(userIdClaimName, userId.ToString()),
                });

        private SigningCredentials CreateSigningCredentials()
            => new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                SecurityAlgorithms.HmacSha256Signature);

    }
}