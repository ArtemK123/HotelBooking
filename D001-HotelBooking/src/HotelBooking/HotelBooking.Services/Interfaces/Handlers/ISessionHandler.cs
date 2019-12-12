using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;

namespace HotelBooking.Services.Interfaces
{
    internal interface ISessionHandler
    {
       Task<ClientData> GetClientAsync(string token);
    }
}