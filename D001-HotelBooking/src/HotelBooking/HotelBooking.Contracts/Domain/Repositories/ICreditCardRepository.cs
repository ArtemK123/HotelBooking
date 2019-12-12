using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;

namespace HotelBooking.Contracts.Domain.Repositories
{
    public interface ICreditCardRepository
    {
        Task<CreditCardData> AddAsync(CreditCardData cardData);

        Task<CreditCardData> GetByIdAsync(int id);

        Task<CreditCardData> GetByNumberAsync(long number);

        Task<IReadOnlyCollection<CreditCardData>> GetByClientAsync(int clientId);

        void Remove(CreditCardData cardData);
    }
}