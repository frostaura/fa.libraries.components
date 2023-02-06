using FrostAura.Libraries.Components.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FrostAura.Libraries.Components.Data.Extensions
{
    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add FrostAura components data access services to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <returns>Application services collection.</returns>
        public static IServiceCollection AddFrostAuraComponentsData(this IServiceCollection services)
        {
            return services
                .AddSingleton<IContentDataAccess, EmbeddedContentDataAccess>()
                .AddSingleton<IClientDataAccess, BlazorDefaultClientDataStore>();
        }
    }
}
