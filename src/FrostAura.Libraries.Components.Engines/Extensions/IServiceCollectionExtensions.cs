using Microsoft.Extensions.DependencyInjection;

namespace FrostAura.Libraries.Components.Engines.Extensions
{
    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add FrostAura components engine services to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <returns>Application services collection.</returns>
        public static IServiceCollection AddFrostAuraComponentsEngines(this IServiceCollection services)
        {
            return services;
        }
    }
}
