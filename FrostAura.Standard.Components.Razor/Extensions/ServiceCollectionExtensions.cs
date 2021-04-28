using FrostAura.Standard.Components.Razor.Interfaces.Navigation;
using FrostAura.Standard.Components.Razor.Interfaces.Resources;
using FrostAura.Standard.Components.Razor.Models.Configuration;
using FrostAura.Standard.Components.Razor.Services.Navigation;
using FrostAura.Standard.Components.Razor.Services.Resources;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrostAura.Standard.Components.Razor.Extensions
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
        public static IServiceCollection AddFrostAuraComponents(this IServiceCollection services, Action<FrostAuraApplicationConfiguration> builder, IConfiguration config)
        {
            // Create default configuration.
            var configuration = new FrostAuraApplicationConfiguration();

            // Cascade desired options with the defaults.
            WireUpIdentityConfiguration(configuration, config);
            builder(configuration);

            services
                .AddAuthentication(config =>
                {
                    config.DefaultScheme = "Cookie";
                    config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookie", config =>
                {
                    // Cookie auth configuration.
                })
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = configuration.IdentityServerUrl;
                    config.ClientId = configuration.AppIdentity;
                    config.ClientSecret = configuration.AppSecret;
                    config.SaveTokens = true;
                    config.ResponseType = "code";
                    config.Events = new OpenIdConnectEvents
                    {
                        OnAccessDenied = context =>
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/");

                            return Task.CompletedTask;
                        }
                    };
                    config.Scope.Add("frostaura.scopes.default");
                    configuration
                        .Scopes
                        .ForEach(s => config.Scope.Add(s));
                });

            // Register default form control mappings.
            Input.DynamicField.RegisterRendererTypeControl<string, InputText>(); 
            Input.DynamicField.RegisterRendererTypeControl<int, InputNumber<int>>();
            Input.DynamicField.RegisterRendererTypeControl<DateTime, InputDate<DateTime>>();
            Input.DynamicField.RegisterRendererTypeControl<bool, InputCheckbox>();

            return services
                .AddScoped<IContentService, EmbeddedContentService>()
                .AddScoped<INavigationService, PageNavigationService>()
                .AddSingleton(configuration);
        }

        /// <summary>
        /// Wire up FrostAura application config.
        /// </summary>
        /// <param name="config">Running application config.</param>
        /// <param name="configuration">Application configuration.</param>
        private static void WireUpIdentityConfiguration(FrostAuraApplicationConfiguration config, IConfiguration configuration)
        {
            var identityServerUrl = configuration.GetValue<string>("Identity:Authority");
            var iconName = configuration.GetValue<string>("FrostAura:IconName");
            var scopes = new List<string>();

            configuration
                .GetSection("Identity:Scopes")
                .Bind(scopes);

            config.AppIdentity = configuration.GetValue<string>("Identity:Audience");
            config.AppName = configuration.GetValue<string>("Identity:Name");
            config.AppIconSvgUrl = $"{identityServerUrl}/vectors/icons/{iconName}";
            config.AppSecret = configuration.GetValue<string>("Identity:Secret");
            config.IdentityServerUrl = identityServerUrl;
            config.GoogleMapsApiKey = configuration.GetValue<string>("FrostAura:GoogleMapsApiKey");

            scopes
                .ToList()
                .ForEach(s => config.Scopes.Add(s));
        }
    }
}
