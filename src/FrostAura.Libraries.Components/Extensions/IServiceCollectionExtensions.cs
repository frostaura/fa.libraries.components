using System;
using FrostAura.Libraries.Components.Interfaces.Navigation;
using FrostAura.Libraries.Components.Interfaces.Resources;
using FrostAura.Libraries.Components.Models.Configuration;
using FrostAura.Libraries.Components.Services.Navigation;
using FrostAura.Libraries.Components.Services.Resources;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

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
        /// <param name="config">Application configuration.</param>
        /// <returns>Application services collection.</returns>
        public static IServiceCollection AddFrostAuraComponents(this IServiceCollection services, Action<FrostAuraApplicationConfiguration> builder)
        {
            // Create default configuration.
            var configuration = new FrostAuraApplicationConfiguration();

            // Cascade desired options with the defaults.
            builder(configuration);

            return services
                .AddScoped<IContentService, EmbeddedContentService>()
                .AddScoped<INavigationService, PageNavigationService>()
                .AddSingleton(configuration);
        }

    }
}
