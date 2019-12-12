using System.Threading.Tasks;
using HotelBooking.Contracts.Dto.Data;

namespace HotelBooking.Contracts.Domain.Repositories
{
    public interface IApplicationRoleRepository
    {
        RoleData Get(string name);

        RoleData Get(int id);
    }
}