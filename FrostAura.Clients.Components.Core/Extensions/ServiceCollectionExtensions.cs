using FrostAura.Standard.Components.Razor.Extensions;
using FrostAura.Standard.Components.Razor.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FrostAura.Clients.Components.Core.Extensions
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
        public static IServiceCollection AddFrostAuraCore(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddServices(config);
        }

        /// <summary>
        /// Add all required application engine services to the DI container.
        /// </summary>
        /// <param name="services">Application services collection.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>Application services collection.</returns>
        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                // Register required services - scoped usually due to Blazor's DI model.
                .AddFrostAuraComponents(c =>
                {
                    // Unauthenticated nav items.
                    c.NavigationItems.Add(new NavLink
                    {
                        IconCssClass = "fa fa-home",
                        Title = "Home",
                        Path = "/",
                        MatchType = Microsoft.AspNetCore.Components.Routing.NavLinkMatch.All
                    });
                    c.NavigationItems.Add(new NavLink
                    {
                        IconCssClass = "fa fa-sign-in",
                        Title = "Sign In",
                        Path = $"identity/login?redirectUri={Uri.EscapeUriString("/secure")}"
                    });

                    // Authenticated nav items.
                    c.NavigationItems.Add(new NavLink
                    {
                        IconCssClass = "fa fa-home",
                        Title = "Secure Home",
                        Path = "/secure",
                        RequireAuthentication = true,
                        MatchType = Microsoft.AspNetCore.Components.Routing.NavLinkMatch.All
                    });
                    c.NavigationItems.Add(new NavLink
                    {
                        IconCssClass = "fa fa-sign-out",
                        Title = "Sign Out",
                        Path = $"identity/logout?redirectUri={Uri.EscapeUriString("/")}",
                        RequireAuthentication = true,
                    });
                }, configuration);
        }
    }
}
