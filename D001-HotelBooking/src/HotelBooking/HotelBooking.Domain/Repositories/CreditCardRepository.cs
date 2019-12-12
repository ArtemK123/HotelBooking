using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;

namespace HotelBooking.Domain.Repositories
{
    internal class CreditCardRepository : ICreditCardRepository
    {
        private readonly IApplicationDbContext context;

        public CreditCardRepository(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<CreditCardData> AddAsync(CreditCardData cardData)
        {
            var insertionResult = await context.CreditCards.AddAsync(Map(cardData));
            context.SaveChanges();
            return Map(insertionResult.Entity);
        }

        public async Task<IReadOnlyCollection<CreditCardData>> GetByClientAsync(int clientId)
            => Map(context.CreditCards.Where((card) => (card.ClientId == clientId)).ToList());

        public async Task<CreditCardData> GetByIdAsync(int id)
            => Map(await context.CreditCards.FindAsync(id));

        public async Task<CreditCardData> GetByNumberAsync(long number)
            => Map(context.CreditCards.FirstOrDefault((card) => card.Number == number));

        public void Remove(CreditCardData cardData)
        {
            CreditCard card = context.CreditCards.FirstOrDefault((record) => record.Number == cardData.Number);
            context.CreditCards.Remove(card);
            context.SaveChanges();
        }

        private IReadOnlyCollection<CreditCardData> Map(IReadOnlyCollection<CreditCard> creditCards)
            => creditCards.Select(Map).ToList();

        private CreditCard Map(CreditCardData cardData)
         => new CreditCard
         {
             CV2 = cardData.CV2,
             DueDate = cardData.DueDate,
             Number = cardData.Number,
             ClientId = cardData.ClientId,
         };

        private CreditCardData Map(CreditCard card)
         => card == null ? null : new CreditCardData
         {
             Id = card.Id,
             CV2 = card.CV2,
             DueDate = card.DueDate,
             Number = card.Number,
             ClientId = card.ClientId,
         };
    }
}