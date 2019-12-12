using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repositories
{
    internal class SessionRepository : ISessionRepository
    {
        private readonly IApplicationDbContext context;

        public SessionRepository(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateAsync(SessionData sessionData)
        {
            var session = new Session
            {
                UserId = sessionData.UserId,
                Token = sessionData.Token
            };

            await context.Sessions.AddAsync(session);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(SessionData sessionData)
        {
            Session session = await context.Sessions.FirstOrDefaultAsync(record =>
                record.Token == sessionData.Token);

            context.Sessions.Remove(session);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<SessionData> GetByTokenAsync(string token)
            => Map(await context.Sessions.FirstOrDefaultAsync(session => session.Token == token));

        private SessionData Map(Session session)
            => (session == null) ? null : new SessionData
            {
                Id = session.Id,
                Token = session.Token,
                UserId = session.UserId,
            };
    }
}