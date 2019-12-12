using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Entities.Identity;
using HotelBooking.Domain.Entities.Users;
using HotelBooking.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain
{
    internal class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<CreditCard> CreditCards { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<HotelImage> HotelImages { get; set; }

        public DbSet<RoomImage> RoomImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Ignore(u => u.AccessFailedCount)
                                  .Ignore(u => u.NormalizedUserName)
                                  .Ignore(u => u.PhoneNumber)
                                  .Ignore(u => u.PhoneNumberConfirmed)
                                  .Ignore(u => u.TwoFactorEnabled);

            builder.Entity<Client>().HasKey(u => u.Id);
            builder.Entity<Manager>().HasKey(u => u.Id);
            builder.Entity<CreditCard>().HasKey(u => u.Id);
            builder.Entity<Hotel>().HasKey(u => u.Id);
            builder.Entity<Room>().HasKey(u => u.Id);
            builder.Entity<Order>().HasKey(u => u.Id);
            builder.Entity<Session>().HasKey(u => u.Id);
            builder.Entity<HotelImage>().HasKey(u => u.Id);
            builder.Entity<RoomImage>().HasKey(u => u.Id);
        }
    }
}