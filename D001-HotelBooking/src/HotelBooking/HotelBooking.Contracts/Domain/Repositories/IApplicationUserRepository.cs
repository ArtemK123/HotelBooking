using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Contracts.Dto.Models.Account;
using HotelBooking.Contracts.Dto.Models.LogIn;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Contracts.Domain.Repositories
{
    public interface IApplicationUserRepository
    {
        Task<IdentityResult> CreateAsync(UserData userDto);

        Task<UserData> FindByEmailAsync(string email);

        Task<UserData> FindByIdAsync(int id);

        Task<bool> CheckPasswordAsync(string email, string password);

        Task<IdentityResult> UpdateAsync(UserData user, string newEmail);

        Task<IdentityResult> ChangePasswordAsync(UpdateClientRequestModel dto);
    }
}