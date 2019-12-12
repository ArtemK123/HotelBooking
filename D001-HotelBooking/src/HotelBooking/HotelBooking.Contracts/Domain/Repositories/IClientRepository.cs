using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;

namespace HotelBooking.Contracts.Domain.Repositories
{
    public interface IClientRepository
    {
        Task<ClientData> AddAsync(ClientData client);

        Task<ClientData> FindByIdAsync(int id);

        ClientData FindByUser(UserData userData);

        Task<bool> UpdateAsync(ClientData client);
    }
}