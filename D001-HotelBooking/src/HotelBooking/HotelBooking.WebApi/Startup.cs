using HotelBooking.Contracts.Domain.Initializers;
using HotelBooking.Contracts.Domain.Seeders;
using HotelBooking.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.WebApi
{
    public class Startup
    {
        private const string AllowAnyOriginPolicyName = "AllowAnyOrigin";
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    AllowAnyOriginPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader();
                    });
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(AllowAnyOriginPolicyName));
            });

            services.AddHotelBooking(configuration);
        }

        public void Configure (
            IApplicationBuilder applicationBuilder,
            IDatabaseInitializer databaseInitializer,
            IDatabaseSeeder databaseSeeder)
        {
            applicationBuilder.UseDeveloperExceptionPage();
            applicationBuilder.UseStaticFiles();

            databaseInitializer.Initialize();
            applicationBuilder.UseCors(AllowAnyOriginPolicyName);

            applicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute("default", "api/{controller}/{action}/{id?}");
            });

            applicationBuilder.UseAuthentication();

            databaseSeeder.Seed();
        }
    }
}