using HotelBooking.Contracts.Domain.Initializers;
using HotelBooking.Domain.Interfaces;

namespace HotelBooking.Domain.Initializers
{
    internal class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IApplicationDbContext context;

        public DatabaseInitializer(IApplicationDbContext context)
        {
            this.context = context;
        }

        public void Initialize()
        {
            context.Database.EnsureCreated();
        }
    }
}