using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Services.Interfaces;

namespace HotelBooking.Services.Handlers
{
    internal class SessionHandler : ISessionHandler
    {
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly ISessionRepository sessionRepository;
        private readonly IClientRepository clientRepository;

        public SessionHandler(
            IApplicationUserRepository applicationUserRepository,
            ISessionRepository sessionRepository,
            IClientRepository clientRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.sessionRepository = sessionRepository;
            this.clientRepository = clientRepository;
        }

        public async Task<ClientData> GetClientAsync(string token)
        {
            SessionData session = await sessionRepository.GetByTokenAsync(token);
            if (session == null)
            {
                return null;
            }

            UserData user = await applicationUserRepository.FindByIdAsync(session.UserId);
            if (user == null)
            {
                return null;
            }

            return clientRepository.FindByUser(user);
        }
    }
}