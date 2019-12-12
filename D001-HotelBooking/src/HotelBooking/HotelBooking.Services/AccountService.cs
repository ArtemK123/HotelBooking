using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.LogIn;
using HotelBooking.Contracts.Dto.Models.Register;
using HotelBooking.Contracts.Services;
using HotelBooking.Services.Interfaces;
using HotelBooking.Services.Interfaces.Factories;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Services
{
    internal class AccountService : IAccountService
    {
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly ISessionRepository sessionRepository;
        private readonly IJavascriptWebTokenFactory javascriptWebTokenFactory;
        private readonly IClientRepository clientRepository;
        private readonly IApplicationRoleRepository applicationRoleRepository;
        private readonly ISessionHandler sessionHandler;
        private readonly ICreditCardRepository creditCardRepository;

        public AccountService(
            IApplicationUserRepository applicationUserRepository,
            ISessionRepository sessionRepository,
            IClientRepository clientRepository,
            IJavascriptWebTokenFactory javascriptWebTokenFactory,
            IApplicationRoleRepository applicationRoleRepository,
            ISessionHandler sessionHandler,
            ICreditCardRepository creditCardRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.sessionRepository = sessionRepository;
            this.javascriptWebTokenFactory = javascriptWebTokenFactory;
            this.clientRepository = clientRepository;
            this.applicationRoleRepository = applicationRoleRepository;
            this.sessionHandler = sessionHandler;
            this.creditCardRepository = creditCardRepository;
        }

        public async Task<RegistrationResponseModel> RegisterAsync(RegistrationRequestModel registrationModel)
        {
            RoleData role = applicationRoleRepository.Get(registrationModel.Role);
            RegistrationResponseModel response = new RegistrationResponseModel() { IsSuccessful = false, Message = string.Empty };
            var userData = new UserData
            {
                Email = registrationModel.Email,
                Password = registrationModel.Password,
                RoleId = role.Id
            };

            IdentityResult userCreatingResult = await applicationUserRepository.CreateAsync(userData);
            if (!userCreatingResult.Succeeded)
            {
                // pushing message of first error in array
                response.Message = GetErrorMessage(userCreatingResult);
                return response;
            }

            userData = await applicationUserRepository.FindByEmailAsync(userData.Email);
            ClientData client = new ClientData()
            {
                Name = registrationModel.UserName,
                Surname = registrationModel.Surname,
                PhotoPath = "default/profile.png",
                UserId = userData.Id
            };
            ClientData addedClient = await clientRepository.AddAsync(client);
            if (addedClient == null)
            {
                response.Message = "Client not added";
            }
            response.IsSuccessful = true;
            string token = javascriptWebTokenFactory.Create(userData.Id);
            var sessionData = new SessionData
            {
                UserId = userData.Id,
                Token = token,
            };
            await sessionRepository.CreateAsync(sessionData);
            response.Token = token;
            return response;
        }

        public async Task<LogInResponseModel> LogInAsync(LogInRequestModel logInRequestModel)
        {
            var response = new LogInResponseModel { IsSuccessful = false };

            UserData userData = await applicationUserRepository.FindByEmailAsync(logInRequestModel.Email.Normalize());

            if (userData == null)
            {
                response.Message = "Account with this email doesn`t exists";
            }
            else if (!await applicationUserRepository.CheckPasswordAsync(
                logInRequestModel.Email,
                logInRequestModel.Password))
            {
                response.Message = "Wrong Password";
            }
            else
            {
                string token = javascriptWebTokenFactory.Create(userData.Id);
                var sessionDto = new SessionData
                {
                    UserId = userData.Id,
                    Token = token
                };
                await sessionRepository.CreateAsync(sessionDto);
                response.Token = token;
                response.IsSuccessful = true;
            }
            return response;
        }

        public async Task<ClientAccountModel> GetClientAccountAsync(string token)
        {
            SessionData session = await sessionRepository.GetByTokenAsync(token);
            UserData user = await applicationUserRepository.FindByIdAsync(session.UserId);
            ClientData client = clientRepository.FindByUser(user);
            var account = new ClientAccountModel()
            {
                ClientId = client.Id,
                Email = user.Email,
                Passport = client.Passport,
                Telephone = client.Telephone,
                Name = client.Name,
                Surname = client.Surname,
                PhotoPath = client.PhotoPath,
                Role = applicationRoleRepository.Get(user.RoleId).Name,
                CreditCards = await creditCardRepository.GetByClientAsync(client.Id)
            };
            return account;
        }

        public async Task<UpdateClientResponseModel> UpdateClientAccountAsync(string token, UpdateClientRequestModel dto)
        {
            var response = new UpdateClientResponseModel { IsSuccessful = false, Message = string.Empty };
            SessionData sessionData = await sessionRepository.GetByTokenAsync(token);
            if (sessionData == null)
            {
                response.Message = "Unauthorized";
                return response;
            }
            UserData user = await applicationUserRepository.FindByIdAsync(sessionData.UserId);
            ClientData client = clientRepository.FindByUser(user);
            if (!await applicationUserRepository.CheckPasswordAsync(dto.Email, dto.OldPassword))
            {
                response.Message = "You should write your current password before update";
                return response;
            }

            user.Email = dto.Email;
            client.Name = dto.Name;
            client.Surname = dto.Surname;
            client.Telephone = dto.Telephone;
            client.Passport = dto.Passport;

            if (dto.NewPassword != null && dto.NewPassword != string.Empty && dto.NewPassword != dto.OldPassword)
            {
                var passwordChangeResult = await applicationUserRepository.ChangePasswordAsync(dto);
                if (!passwordChangeResult.Succeeded)
                {
                    response.Message = "Error while changing passsword";
                    return response;
                }
                response.Message = "Password changed successfully";
            }
            bool clientRes = await clientRepository.UpdateAsync(client);

            if (!clientRes)
            {
                response.Message = "Error while updating client information";
            }
            else
            {
                response.IsSuccessful = true;
            }
            return response;
        }

        public async Task<bool> LogoutAsync(SessionData sessionData) => await sessionRepository.RemoveAsync(sessionData);

        public async Task<DefaultResponseModel> AddCreditCardAsync(string token, CreditCardData creditCardData)
        {
            DefaultResponseModel response = new DefaultResponseModel { IsSuccessful = false, Message = string.Empty };
            ClientData client = await sessionHandler.GetClientAsync(token);
            if (client == null)
            {
                response.Message = "Unauthorized user";
                return response;
            }
            CreditCardData cardInDatabase = await creditCardRepository.GetByNumberAsync(creditCardData.Number);
            if (cardInDatabase != null)
            {
                response.Message = "This card already registered in the database";
                return response;
            }
            creditCardData.ClientId = client.Id;
            creditCardData = await creditCardRepository.AddAsync(creditCardData);
            if (creditCardData.Id > 0)
            {
                response.IsSuccessful = true;
            }
            return response;
        }

        public async Task<DefaultResponseModel> RemoveCreditCardAsync(string token, CreditCardData creditCardData)
        {
            DefaultResponseModel response = new DefaultResponseModel { IsSuccessful = false, Message = string.Empty };
            ClientData client = await sessionHandler.GetClientAsync(token);
            if (client == null)
            {
                response.Message = "Unauthorized user";
                return response;
            }
            IReadOnlyCollection<CreditCardData> storedCards = await creditCardRepository.GetByClientAsync(client.Id);
            if (!storedCards.Any((card) => card.Number == creditCardData.Number))
            {
                response.Message = "Client doesn`t have this card";
                return response;
            }
            creditCardRepository.Remove(creditCardData);
            response.IsSuccessful = true;
            return response;
        }

        private string GetErrorMessage(IdentityResult identityResult)
        {
            // return first error in list
            var errorsEnumarator = identityResult.Errors.GetEnumerator();
            errorsEnumarator.MoveNext();
            return errorsEnumarator.Current.Description;
        }
    }
}