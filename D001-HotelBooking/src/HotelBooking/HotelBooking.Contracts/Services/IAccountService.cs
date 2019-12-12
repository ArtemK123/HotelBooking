using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.LogIn;
using HotelBooking.Contracts.Dto.Models.Register;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Contracts.Services
{
    public interface IAccountService
    {
        Task<RegistrationResponseModel> RegisterAsync(RegistrationRequestModel registrationRequestModel);

        Task<LogInResponseModel> LogInAsync(LogInRequestModel logInRequestModel);

        Task<ClientAccountModel> GetClientAccountAsync(string token);

        Task<UpdateClientResponseModel> UpdateClientAccountAsync(string token, UpdateClientRequestModel model);

        Task<DefaultResponseModel> AddCreditCardAsync(string token, CreditCardData creditCardData);

        Task<DefaultResponseModel> RemoveCreditCardAsync(string token, CreditCardData creditCardData);

        Task<bool> LogoutAsync(SessionData data);
    }
}