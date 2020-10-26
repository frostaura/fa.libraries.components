using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FrostAura.Clients.Components.Data.Extensions
{
    /// <summary>
    /// Extensions for IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add all required application engine and manager services and config to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <param name="config">Configuration for the application.</param>
        /// <returns>Application services collection.</returns>
        public static IServiceCollection AddFrostAuraResources(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddServices(config);
        }

        /// <summary>
        /// Add all required application engine services to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <param name="config">Configuration for the application.</param>
        /// <returns>Application services collection.</returns>
        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("ApplicationDbContext");
            var migrationsAssembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name;

            return services
                .AddDbContext<ApplicationDbContext>(config =>
                {
                    config.UseSqlServer(connectionString);
                });
        }
    }
}
