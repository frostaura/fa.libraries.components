using FrostAura.Libraries.Components.Shared.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FrostAura.Libraries.Components.Managers.Extensions;
using FrostAura.Libraries.Components.Engines.Extensions;
using FrostAura.Libraries.Components.Data.Extensions;

namespace FrostAura.Libraries.Components.Extensions
{
    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add FrostAura components to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <param name="builder">Configuration builder.</param>
        /// <returns>Application services collection.</returns>
        public static IServiceCollection AddFrostAuraComponents(this IServiceCollection services, Action<FrostAuraApplicationConfiguration> builder)
        {
            // Create default configuration.
            var configuration = new FrostAuraApplicationConfiguration();

            // Cascade desired options with the defaults.
            builder(configuration);

            var newServices = services
                .AddFrostAuraComponentsManagers()
                .AddFrostAuraComponentsEngines()
                .AddFrostAuraComponentsData();

            newServices.AddHttpClient("default", c =>
            {
                c.BaseAddress = new Uri(configuration.AppBaseUrl);
            });

            return newServices;
        }
    }
}
