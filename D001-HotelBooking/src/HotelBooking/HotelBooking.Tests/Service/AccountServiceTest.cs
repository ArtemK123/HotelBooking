using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.Register;
using HotelBooking.Services;
using HotelBooking.Services.Interfaces;
using HotelBooking.Services.Interfaces.Factories;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.Service
{
    [TestFixture]
    internal class AccountServiceTest : UnitTestBase
    {
        private const string UpdateClientAccountAsyncMethodName = nameof(AccountService.UpdateClientAccountAsync) + ". ";
        private const string GetClientAccountAsyncMethodName = nameof(AccountService.GetClientAccountAsync) + ". ";
        private const string RegisterAsyncMethodName = nameof(AccountService.RegisterAsync) + ". ";
        private const string LogoutAsyncMethodName = nameof(AccountService.LogoutAsync) + ". ";

        private const bool Successful = true;
        private const string JavascriptWebToken = "token";

        private Mock<IApplicationUserRepository> applicationUserRepositoryMock;
        private Mock<ISessionRepository> sessionRepositoryMock;
        private Mock<IClientRepository> clientRepositoryMock;
        private Mock<IJavascriptWebTokenFactory> javascriptWebTokenFactoryMock;
        private Mock<IApplicationRoleRepository> applicationRoleRepositoryMock;
        private Mock<ISessionHandler> sessionHandlerMock;
        private Mock<ICreditCardRepository> creditCardRepository;

        private AccountService accountService;

        [SetUp]
        public void TestInitialize()
        {
            applicationUserRepositoryMock = MockRepository.Create<IApplicationUserRepository>();
            sessionRepositoryMock = MockRepository.Create<ISessionRepository>();
            clientRepositoryMock = MockRepository.Create<IClientRepository>();
            javascriptWebTokenFactoryMock = MockRepository.Create<IJavascriptWebTokenFactory>();
            applicationRoleRepositoryMock = MockRepository.Create<IApplicationRoleRepository>();
            sessionHandlerMock = MockRepository.Create<ISessionHandler>();
            creditCardRepository = MockRepository.Create<ICreditCardRepository>();

            accountService = new AccountService(
                applicationUserRepositoryMock.Object,
                sessionRepositoryMock.Object,
                clientRepositoryMock.Object,
                javascriptWebTokenFactoryMock.Object,
                applicationRoleRepositoryMock.Object,
                sessionHandlerMock.Object,
                creditCardRepository.Object);
        }

        [TestCase(TestName = RegisterAsyncMethodName + "Should return result from applicationUserRepository CreateAsync method")]
        public async Task RegisterAsyncTest()
        {
            RoleData roleData = CreateRoleData();
            UserData userData = CreateUserData();
            IdentityResult identityResult = CreateIdentityResult(Successful);
            ClientData startingClientData = CreateClientData();
            ClientData resultingClientData = CreateClientData();
            SessionData sessionData = CreateSessionData();
            RegistrationRequestModel registrationRequestModel = CreateRegistrationRequestModel();


            SetupApplicationRoleRepositoryGetByRoleNameMock(roleData.Name, roleData);
            SetupApplicationUserRepositoryCreateAsyncMock(userData, identityResult);
            SetupApplicationUserRepositoryFindByEmailAsyncMock(userData);
            SetupClientRepositoryAddAsyncMock(startingClientData, resultingClientData);
            SetupJavascriptWebTokenFactoryCreateMock(userData.Id, JavascriptWebToken);
            SetupSessionRepositoryCreateAsyncMock(sessionData, Successful);

            RegistrationResponseModel expected = CreateSuccessfulRegistrationResponseModel();
            RegistrationResponseModel actual = await accountService.RegisterAsync(registrationRequestModel);

            Assert.AreEqual(actual.IsSuccessful, expected.IsSuccessful);
            Assert.AreEqual(actual.Message, expected.Message);
        }

        [TestCase(TestName = GetClientAccountAsyncMethodName + "Shoult return a task with a ClientAccountModel when the token is valid")]
        public async Task GetClientAccountAsync()
        {
            SessionData sessionData = CreateSessionData();
            UserData userData = CreateUserData();
            ClientData clientData = CreateClientData();
            RoleData roleData = CreateRoleData();
            IReadOnlyCollection<CreditCardData> creditCards = CreateCreditCards();

            SetupSessionRepositoryMock(JavascriptWebToken, sessionData);
            SetupApplicationUserRepositoryFindByIdAsyncMock(sessionData.UserId, userData);
            SetupClientRepositoryFindByUserMock(userData, clientData);
            SetupApplicationRoleRepositoryGetByIdMock(userData.RoleId, roleData);
            SetupCreditCardRepositoryGetByClientAsyncMock(clientData.Id, creditCards);

            var expected = CreateClientAccountModel(userData, clientData, roleData, creditCards);
            var actual = await accountService.GetClientAccountAsync(JavascriptWebToken);

            Assert.AreEqual(expected.ClientId, actual.ClientId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Passport, actual.Passport);
            Assert.AreEqual(expected.Telephone, actual.Telephone);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Surname, actual.Surname);
            Assert.AreEqual(expected.PhotoPath, actual.PhotoPath);
            Assert.AreEqual(expected.Role, actual.Role);
            Assert.AreEqual(expected.CreditCards.GetHashCode(), actual.CreditCards.GetHashCode());
        }

        [TestCase(TestName = UpdateClientAccountAsyncMethodName
            + "Should return a task with UpdateResposeModel with information about" +
            " the successful update without changing password when token and request dto are valid and new password is empty")]
        public async Task UpdateClientAccountWithOldPasswordAsyncTest()
        {
            UpdateClientRequestModel dto = CreateValidUpdateClientRequestModel();
            SessionData sessionData = CreateSessionData();
            UserData userData = CreateUserData();
            ClientData clientData = CreateClientData();

            SetupSessionRepositoryMock(JavascriptWebToken, sessionData);
            SetupApplicationUserRepositoryFindByIdAsyncMock(sessionData.UserId, userData);
            SetupClientRepositoryFindByUserMock(userData, clientData);
            SetupApplicationUserRepositoryCheckPasswordAsyncMock(dto.Email, dto.OldPassword, Successful);
            SetupClientRepositoryUpdateAsyncMock(clientData, Successful);

            var expected = CreateSuccessfulUpdateResponseModelWithoutChangingPassword();
            var actual = await accountService.UpdateClientAccountAsync(JavascriptWebToken, dto);

            Assert.AreEqual(expected.IsSuccessful, actual.IsSuccessful);
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestCase(TestName = LogoutAsyncMethodName + "Should return a task with bool variable, " +
            "which represents success of the operation")]
        public async Task LogoutAsync()
        {
            SessionData sessionData = CreateSessionData();
            SetupSessionRepositoryRemoveAsyncMock(sessionData, Successful);
            var actual = await accountService.LogoutAsync(sessionData);
            Assert.IsTrue(actual);
        }

        private void SetupSessionRepositoryMock(string token, SessionData sessionData)
            => sessionRepositoryMock
                .Setup(repository => repository.GetByTokenAsync(token))
                .ReturnsAsync(sessionData);

        // UserRepository
        private void SetupApplicationUserRepositoryCheckPasswordAsyncMock(string email, string password, bool result)
            => applicationUserRepositoryMock
                .Setup(repository => repository.CheckPasswordAsync(email, password))
                .ReturnsAsync(result);

        private void SetupApplicationUserRepositoryFindByIdAsyncMock(int id, UserData userData)
            => applicationUserRepositoryMock
                .Setup(repository => repository.FindByIdAsync(id))
                .ReturnsAsync(userData);

        private void SetupApplicationUserRepositoryCreateAsyncMock(UserData userData, IdentityResult identityResult)
            => applicationUserRepositoryMock
               .Setup(repository => repository.CreateAsync(
                   It.Is<UserData>(currentUserData => 
                        currentUserData.Email == userData.Email &&
                        currentUserData.Password == userData.Password &&
                        currentUserData.RoleId == userData.Id
                   )))
                .ReturnsAsync(identityResult);

        private void SetupApplicationUserRepositoryFindByEmailAsyncMock(UserData userData)
            => applicationUserRepositoryMock
                .Setup(repository => repository.FindByEmailAsync(userData.Email))
                .ReturnsAsync(userData);

        // ClientRepository
        private void SetupClientRepositoryFindByUserMock(UserData userData, ClientData clientData)
            => clientRepositoryMock
                 .Setup(repository => repository.FindByUser(userData))
                 .Returns(clientData);

        private void SetupClientRepositoryAddAsyncMock(ClientData startingClientData, ClientData resultingClientData)
            => clientRepositoryMock
                .Setup(repository => repository.AddAsync(
                    It.Is<ClientData>(currentClientData =>
                        currentClientData.Id == startingClientData.Id &&
                        currentClientData.Name == startingClientData.Name &&
                        currentClientData.Surname == startingClientData.Surname &&
                        currentClientData.Telephone == startingClientData.Telephone &&
                        currentClientData.Passport == startingClientData.Passport &&
                        currentClientData.PhotoPath == startingClientData.PhotoPath &&
                        currentClientData.UserId == startingClientData.UserId
                    )))
                .ReturnsAsync(resultingClientData);

        private void SetupClientRepositoryUpdateAsyncMock(ClientData clientData, bool success)
            => clientRepositoryMock
                .Setup(repository => repository.UpdateAsync(clientData))
                .ReturnsAsync(success);

        // RoleRepository
        private void SetupApplicationRoleRepositoryGetByRoleNameMock(string name, RoleData roleData)
            => applicationRoleRepositoryMock
                .Setup(repository => repository.Get(name))
                .Returns(roleData);

        private void SetupApplicationRoleRepositoryGetByIdMock(int id, RoleData roleData)
           => applicationRoleRepositoryMock
               .Setup(repository => repository.Get(id))
               .Returns(roleData);

        // SessionRepository
        private void SetupSessionRepositoryCreateAsyncMock(SessionData sessionData, bool success)
            => sessionRepositoryMock
                .Setup(repository => repository.CreateAsync(
                     It.Is<SessionData>(currentSessionData =>
                        currentSessionData.Id == sessionData.Id &&
                        currentSessionData.Token == sessionData.Token &&
                        currentSessionData.UserId == sessionData.UserId
                    )))
                .ReturnsAsync(success);

        private void SetupSessionRepositoryRemoveAsyncMock(SessionData sessionData, bool success)
            => sessionRepositoryMock
                .Setup(repository => repository.RemoveAsync(sessionData))
                .ReturnsAsync(success);

        // CreditCardsrepository
        private void SetupCreditCardRepositoryGetByClientAsyncMock(int clientId, IReadOnlyCollection<CreditCardData> creditCards)
            => creditCardRepository
                .Setup(repository => repository.GetByClientAsync(clientId))
                .ReturnsAsync(creditCards);

        // JavascriptWebTokenFactory
        private void SetupJavascriptWebTokenFactoryCreateMock(int userId, string token)
            => javascriptWebTokenFactoryMock
                .Setup(repository => repository.Create(userId))
                .Returns(token);

        // Data Factories
        private RoleData CreateRoleData()
            => new RoleData();

        private UserData CreateUserData()
            => new UserData();

        private IdentityResult CreateIdentityResult(bool success)
            => new IdentityResultMock(success);

        private ClientData CreateClientData()
            => new ClientData { PhotoPath = "default/profile.png" };

        private SessionData CreateSessionData()
            => new SessionData { Token = JavascriptWebToken };

        private IReadOnlyCollection<CreditCardData> CreateCreditCards()
            => new List<CreditCardData>();

        // Model Factories
        private UpdateClientRequestModel CreateValidUpdateClientRequestModel()
            => new UpdateClientRequestModel();

        private UpdateClientResponseModel CreateSuccessfulUpdateResponseModelWithoutChangingPassword()
            => new UpdateClientResponseModel { IsSuccessful = true, Message = string.Empty };

        private RegistrationRequestModel CreateRegistrationRequestModel()
            => new RegistrationRequestModel();

        private RegistrationResponseModel CreateSuccessfulRegistrationResponseModel()
            => new RegistrationResponseModel() { IsSuccessful = true, Message = string.Empty };

        private ClientAccountModel CreateClientAccountModel(UserData user, ClientData client, RoleData role, IReadOnlyCollection<CreditCardData> credtiCards)
            => new ClientAccountModel {
                ClientId = client.Id,
                Email = user.Email,
                Passport = client.Passport,
                Telephone = client.Telephone,
                Name = client.Name,
                Surname = client.Surname,
                PhotoPath = client.PhotoPath,
                Role = role.Name,
                CreditCards = credtiCards
            };
    }
}