using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;

namespace HotelBooking.Contracts.Domain.Repositories
{
    public interface ISessionRepository
    {
        Task<bool> CreateAsync(SessionData sessionData);

        Task<bool> RemoveAsync(SessionData sessionData);

        Task<SessionData> GetByTokenAsync(string token);
    }
}