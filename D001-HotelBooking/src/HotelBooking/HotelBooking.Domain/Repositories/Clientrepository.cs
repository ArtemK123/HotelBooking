using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;
using HotelBooking.Domain.Entities.Users;
using HotelBooking.Domain.Interfaces;

namespace HotelBooking.Domain.Repositories
{
    internal class ClientRepository : IClientRepository
    {
        private readonly IApplicationDbContext context;
        private readonly IApplicationUserRepository applicationUserRepository;

        public ClientRepository(IApplicationDbContext context, IApplicationUserRepository applicationUserRepository)
        {
            this.context = context;
            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task<ClientData> AddAsync(ClientData clientData)
        {
            var creatingResult = await context.Clients.AddAsync(Map(clientData));
            await context.SaveChangesAsync();
            return Map(creatingResult.Entity);
        }

        public async Task<ClientData> FindByIdAsync(int id)
            => Map(await context.Clients.FindAsync(id));

        public ClientData FindByUser(UserData userData)
        {
            var user = context.Clients.FirstOrDefault((client) => client.UserId == userData.Id);

            return Map(user);
        }

        public async Task<bool> UpdateAsync(ClientData clientData)
        {
            Client client = await context.Clients.FindAsync(clientData.Id);
            client.Name = clientData.Name;
            client.Surname = clientData.Surname;
            client.Passport = clientData.Passport;
            client.Telephone = clientData.Telephone;
            client.PhotoPath = clientData.PhotoPath;

            context.Clients.Update(client);
            await context.SaveChangesAsync();
            return true;
        }

        private ClientData Map(Client client)
            => new ClientData
            {
                Id = client.Id,
                Name = client.Name,
                Surname = client.Surname,
                Passport = client.Passport,
                Telephone = client.Telephone,
                PhotoPath = client.PhotoPath,
                UserId = client.UserId,
            };

        private Client Map(ClientData client)
            => new Client
            {
                Name = client.Name,
                Surname = client.Surname,
                Passport = client.Passport,
                Telephone = client.Telephone,
                PhotoPath = client.PhotoPath,
                UserId = client.UserId,
            };
    }
}