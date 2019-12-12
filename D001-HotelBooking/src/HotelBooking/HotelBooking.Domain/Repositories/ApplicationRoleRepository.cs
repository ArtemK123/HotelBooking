using System.Collections.Generic;
using System.Linq;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Dto.Data;

namespace HotelBooking.Domain.Repositories
{
    internal class ApplicationRoleRepository : IApplicationRoleRepository
    {
        private readonly IReadOnlyCollection<RoleData> roles;

        public ApplicationRoleRepository()
        {
            var clientRole = new RoleData
            {
                Name = "client",
                Id = 1
            };
            var managerRole = new RoleData
            {
                Name = "manager",
                Id = 2
            };
            this.roles = new List<RoleData> { clientRole, managerRole };
        }

        public RoleData Get(string name)
            => roles.FirstOrDefault((role) => role.Name == name.ToLower());

        public RoleData Get(int id) => roles.ElementAt(id - 1);
    }
}