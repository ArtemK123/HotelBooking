using System.Threading;
using System.Threading.Tasks;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HotelBooking.Domain.Interfaces
{
    internal interface IApplicationDbContext
    {
        DbSet<Client> Clients { get; set; }

        DbSet<Manager> Managers { get; set; }

        DbSet<CreditCard> CreditCards { get; set; }

        DbSet<Hotel> Hotels { get; set; }

        DbSet<Room> Rooms { get; set; }

        DbSet<Order> Orders { get; set; }

        DbSet<Session> Sessions { get; set; }

        DbSet<HotelImage> HotelImages { get; set; }

        DbSet<RoomImage> RoomImages { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        int SaveChanges();
    }
}