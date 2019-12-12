using System.IdentityModel.Tokens.Jwt;
using HotelBooking.Contracts.Domain.Initializers;
using HotelBooking.Contracts.Domain.Repositories;
using HotelBooking.Contracts.Domain.Seeders;
using HotelBooking.Contracts.Services;
using HotelBooking.Domain;
using HotelBooking.Domain.Entities.Identity;
using HotelBooking.Domain.Initializers;
using HotelBooking.Domain.Interfaces;
using HotelBooking.Domain.Repositories;
using HotelBooking.Services;
using HotelBooking.Services.Factories;
using HotelBooking.Services.Handlers;
using HotelBooking.Services.Interfaces;
using HotelBooking.Services.Interfaces.Factories;
using HotelBooking.Services.Interfaces.Handlers;
using HotelBooking.Services.Interfaces.Providers;
using HotelBooking.Services.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.Module
{
    public static class ServiceCollectionExtensionMethods
    {
        public static IConfiguration Configuration { get; }

        public static void AddHotelBooking(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            serviceCollection.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            serviceCollection.AddTransient<IDatabaseSeeder, DatabaseSeeder>();
            serviceCollection.AddTransient<JwtSecurityTokenHandler>();

            BindProviders(serviceCollection);
            BindFactories(serviceCollection);
            BindRepositories(serviceCollection);
            BindServices(serviceCollection);
            BindHandlers(serviceCollection);

            serviceCollection
                .AddIdentity<User, IdentityRole<int>>(options => { options.User.RequireUniqueEmail = true; })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            serviceCollection.AddDbContext<ApplicationDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            serviceCollection.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            });
        }

        private static void BindHandlers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISessionHandler, SessionHandler>();
            serviceCollection.AddTransient<IRoomAvailabilityHandler, RoomAvailabilityHandler>();
        }

        private static void BindProviders(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDateTimeProvider, DateTimeProvider>();
        }

        private static void BindFactories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IJavascriptWebTokenFactory, JavascriptWebTokenFactory>();
            serviceCollection.AddTransient<ISecurityTokenDescriptorFactory, SecurityTokenDescriptorFactory>();
        }

        private static void BindRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            serviceCollection.AddSingleton<IApplicationRoleRepository, ApplicationRoleRepository>();
            serviceCollection.AddTransient<ISessionRepository, SessionRepository>();
            serviceCollection.AddTransient<IHotelRepository, HotelRepository>();
            serviceCollection.AddTransient<IOrderRepository, OrderRepository>();
            serviceCollection.AddTransient<IClientRepository, ClientRepository>();
            serviceCollection.AddTransient<IRoomRepository, RoomRepository>();
            serviceCollection.AddTransient<ICreditCardRepository, CreditCardRepository>();
        }

        private static void BindServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAccountService, AccountService>();
            serviceCollection.AddTransient<IHotelService, HotelService>();
            serviceCollection.AddTransient<IRoomService, RoomService>();
            serviceCollection.AddTransient<IOrderService, OrderService>();
            serviceCollection.AddTransient<IImageService, ImageService>();
        }
    }
}