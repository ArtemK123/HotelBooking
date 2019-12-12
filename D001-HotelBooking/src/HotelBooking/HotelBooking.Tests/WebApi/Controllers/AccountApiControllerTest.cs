using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.LogIn;
using HotelBooking.Contracts.Dto.Models.Register;
using HotelBooking.Contracts.Services;
using HotelBooking.Services.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HotelBooking.Tests.WebApi.Controllers
{
    [TestFixture]
    internal class AccountApiControllerTest : UnitTestBase
    {
        private const string RegisterMethodName = nameof(AccountApiController.Register) + ". ";
        private const string LoginMethodName = nameof(AccountApiController.Login) + ". ";
        private const string LogoutMethodName = nameof(AccountApiController.Logout) + ". ";
        private const string AddCreditCardMethodName = nameof(AccountApiController.AddCreditCard) + ". ";
        private const string RemoveCreditCardMethodName = nameof(AccountApiController.RemoveCreditCard) + ". ";
        private const string GetClientAccountMethodName = nameof(AccountApiController.GetClientAccount) + ". ";
        private const string UpdateClientAccountMethodName = nameof(AccountApiController.UpdateClientAccount) + ". ";

        private const int StatusCode = 200;
        private const string Token = "Token";
        private const bool Successful = true;

        private Mock<IAccountService> accountServiceMock;

        private AccountApiController accountApiController;

        [SetUp]
        public void TestInitialize()
        {
            accountServiceMock = MockRepository.Create<IAccountService>();
            accountApiController = new AccountApiController(accountServiceMock.Object);
        }

        [TestCase(TestName = RegisterMethodName + "Should return JSON form of result got from accountService Register method")]
        public async Task RegisterTest()
        {
            RegistrationRequestModel registrationRequestModel = CreateRegistrationRequestModel();
            RegistrationResponseModel expected = CreateRegistrationResponseModel();

            SetupAccountServiceRegisterMock(registrationRequestModel, expected);

            var actual = (JsonResult)await accountApiController.Register(registrationRequestModel);

            Assert.AreEqual(expected, actual.Value);
        }

        [TestCase(TestName = LoginMethodName + "Should return JSON form of result got from accountService Login method")]
        public async Task LoginTest()
        {
            LogInRequestModel logInRequestModel = CreateLogInRequestModel();
            LogInResponseModel expected = CreateLogInResponseModel();

            SetupAccountServiceLoginMock(logInRequestModel, expected);

            var actual = (JsonResult)await accountApiController.Login(logInRequestModel);

            Assert.AreEqual(expected, actual.Value);
        }

        [TestCase(TestName = LogoutMethodName + "Should return JSON form of result got from accountService Logout method")]
        public async Task LogoutTest()
        {
            SessionData sessionData = CreateSessionData();

            SetupAccountServiceLogoutMock(sessionData, Successful);

            var actual = await accountApiController.Logout(sessionData);

            Assert.AreEqual((actual as OkResult).StatusCode, StatusCode);
        }

        [TestCase(TestName = AddCreditCardMethodName + "Should return JSON form of result got from accountService AddCreditCard method")]
        public async Task AddCreditCardTest()
        {
            CreditCardData creditCardData = CreateCreditCardData();
            DefaultResponseModel expected = CreateDefaultResponseModel();

            SetupAccountServiceAddCreditCardMock(Token, creditCardData, expected);

            var actual = (JsonResult)await accountApiController.AddCreditCard(Token, creditCardData);

            Assert.AreEqual(expected, actual.Value);
        }

        [TestCase(TestName = RemoveCreditCardMethodName + "Should return JSON form of result got from accountService RemoveCreditCard method")]
        public async Task RemoveCreditCardTest()
        {
            CreditCardData creditCardData = CreateCreditCardData();
            DefaultResponseModel expected = CreateDefaultResponseModel();

            SetupAccountServiceRemoveCreditCardMock(Token, creditCardData, expected);

            var actual = (JsonResult)await accountApiController.RemoveCreditCard(Token, creditCardData);

            Assert.AreEqual(expected, actual.Value);
        }

        [TestCase(TestName = GetClientAccountMethodName + "Should return JSON form of result got from accountService GetClientAccount method")]
        public async Task GetClientAccountTest()
        {
            ClientAccountModel expected = CreateClientAccountModel();

            SetupAccountServiceGetClientAccountMock(Token, expected);

            var actual = (JsonResult)await accountApiController.GetClientAccount(Token);

            Assert.AreEqual(expected, actual.Value);
        }

        [TestCase(TestName = UpdateClientAccountMethodName + "Should return JSON form of result got from accountService UpdateClientAccount method")]
        public async Task UpdateClientAccountTest()
        {
            UpdateClientRequestModel updateClientRequestModel = CreateUpdateClientRequestModel();
            UpdateClientResponseModel expected = CreateUpdateClientResponseModel();

            SetupAccountUpdateClientAccountMock(Token, updateClientRequestModel, expected);

            var actual = (JsonResult)await accountApiController.UpdateClientAccount(Token, updateClientRequestModel);

            Assert.AreEqual(expected, actual.Value);
        }

        private void SetupAccountUpdateClientAccountMock(string token, UpdateClientRequestModel updateClientRequestModel, UpdateClientResponseModel updateClientResponseModel)
               => accountServiceMock
                   .Setup(service => service.UpdateClientAccountAsync(token, updateClientRequestModel))
                   .ReturnsAsync(updateClientResponseModel);

        private void SetupAccountServiceGetClientAccountMock(string token, ClientAccountModel clientAccountModel)
               => accountServiceMock
                   .Setup(service => service.GetClientAccountAsync(token))
                   .ReturnsAsync(clientAccountModel);

        private void SetupAccountServiceRemoveCreditCardMock(string token, CreditCardData creditCardData, DefaultResponseModel defaultResponseModel)
               => accountServiceMock
                   .Setup(service => service.RemoveCreditCardAsync(token, creditCardData))
                   .ReturnsAsync(defaultResponseModel);

        private void SetupAccountServiceAddCreditCardMock(string token, CreditCardData creditCardData, DefaultResponseModel defaultResponseModel)
                => accountServiceMock
                    .Setup(service => service.AddCreditCardAsync(token, creditCardData))
                    .ReturnsAsync(defaultResponseModel);

        private void SetupAccountServiceLogoutMock(SessionData sessionData, bool success)
            => accountServiceMock
                .Setup(service => service.LogoutAsync(sessionData))
                .ReturnsAsync(success);

        private void SetupAccountServiceRegisterMock(RegistrationRequestModel registrationRequestModel, RegistrationResponseModel registrationResponseModel)
                => accountServiceMock
                    .Setup(service => service.RegisterAsync(registrationRequestModel))
                    .ReturnsAsync(registrationResponseModel);

        private void SetupAccountServiceLoginMock(LogInRequestModel logInRequestModel, LogInResponseModel logInResponseModel)
               => accountServiceMock
                   .Setup(service => service.LogInAsync(logInRequestModel))
                   .ReturnsAsync(logInResponseModel);

        private UpdateClientResponseModel CreateUpdateClientResponseModel()
            => new UpdateClientResponseModel();

        private UpdateClientRequestModel CreateUpdateClientRequestModel()
            => new UpdateClientRequestModel();

        private ClientAccountModel CreateClientAccountModel()
            => new ClientAccountModel();

        private DefaultResponseModel CreateDefaultResponseModel()
            => new DefaultResponseModel();

        private CreditCardData CreateCreditCardData()
            => new CreditCardData();

        private SessionData CreateSessionData()
            => new SessionData();

        private LogInResponseModel CreateLogInResponseModel()
            => new LogInResponseModel();

        private LogInRequestModel CreateLogInRequestModel()
            => new LogInRequestModel();

        private RegistrationResponseModel CreateRegistrationResponseModel()
            => new RegistrationResponseModel();

        private RegistrationRequestModel CreateRegistrationRequestModel()
            => new RegistrationRequestModel();
    }
}