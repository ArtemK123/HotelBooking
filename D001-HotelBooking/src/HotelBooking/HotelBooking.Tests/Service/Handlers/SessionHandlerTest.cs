using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Services.Handlers;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.Service.Handlers
{
    [TestFixture]
    internal class SessionHandlerTest : UnitTestBase
    {
        private const string GetClientMethodName = nameof(SessionHandler.GetClientAsync) + ". ";
        private const string Token = "Token";
        private const int UserId = 3;

        private SessionHandler sessionHandler;

        private Mock<IApplicationUserRepository> applicationUserRepositoryMock;
        private Mock<ISessionRepository> sessionRepositoryMock;
        private Mock<IClientRepository> clientRepositoryMock;

        [SetUp]
        public void TestInitialize()
        {
            applicationUserRepositoryMock = MockRepository.Create<IApplicationUserRepository>();
            sessionRepositoryMock = MockRepository.Create<ISessionRepository>();
            clientRepositoryMock = MockRepository.Create<IClientRepository>();
            sessionHandler = new SessionHandler(applicationUserRepositoryMock.Object, sessionRepositoryMock.Object, clientRepositoryMock.Object);
        }

        [TestCase(TestName = GetClientMethodName + "Should return valid ClientData")]
        public async Task GetClientTest()
        {
            SessionData sessionData = CreateSessionData();
            UserData userData = CreateUserData();
            ClientData expected = CreateClientData();

            SetupSessionRepositoryMock(sessionData);
            SetupApplicationUserRepositoryMock(sessionData, userData);
            SetupClientRepositoryMock(userData, expected);

            ClientData actual = await sessionHandler.GetClientAsync(Token);

            Assert.AreEqual(actual, expected);
        }

        [TestCase(TestName = GetClientMethodName + "Should return null when SessionData is null")]
        public async Task GetClientNullSessionTest()
        {
            SetupSessionRepositoryMock(null);

            ClientData actual = await sessionHandler.GetClientAsync(Token);

            Assert.IsNull(actual);
        }

        [TestCase(TestName = GetClientMethodName + "Should return null when UserData is null")]
        public async Task GetClientNullUserTest()
        {
            SessionData sessionData = CreateSessionData();
            SetupSessionRepositoryMock(sessionData);
            SetupApplicationUserRepositoryMock(sessionData, null);

            ClientData actual = await sessionHandler.GetClientAsync(Token);

            Assert.IsNull(actual);
        }

        private void SetupSessionRepositoryMock(SessionData sessionData)
            => sessionRepositoryMock
                .Setup(repository => repository.GetByTokenAsync(Token))
                .ReturnsAsync(sessionData);

        private void SetupApplicationUserRepositoryMock(SessionData sessionData, UserData userData)
            => applicationUserRepositoryMock
                .Setup(repository => repository.FindByIdAsync(sessionData.UserId))
                .ReturnsAsync(userData);

        private void SetupClientRepositoryMock(UserData userData, ClientData clientData)
            => clientRepositoryMock
                .Setup(repository => repository.FindByUser(userData))
                .Returns(clientData);

        private SessionData CreateSessionData()
            => new SessionData { UserId = UserId };

        private UserData CreateUserData()
            => new UserData();

        private ClientData CreateClientData()
            => new ClientData();
    }
}