using Microsoft.Extensions.DependencyInjection;

namespace FrostAura.Libraries.Components.Managers.Extensions
{
    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add FrostAura components manager services to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <returns>Application services collection.</returns>
        public static IServiceCollection AddFrostAuraComponentsManagers(this IServiceCollection services)
        {
            return services;
        }
    }
}
