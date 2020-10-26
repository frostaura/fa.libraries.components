using FrostAura.Standard.Components.Razor.Abstractions;
using FrostAura.Standard.Components.Razor.Models.Configuration;
using System.Linq;

namespace FrostAura.Standard.Components.Razor.Extensions
{
    /// <summary>
    /// FrostAura application configuration extensions.
    /// </summary>
    public static class ApplicationConfigurationExtensions
    {
        /// <summary>
        /// Generate and add component nav items to the collection.
        /// </summary>
        /// <param name="configuration">Configuration to add to nav items to.</param>
        /// <returns>Chainable configuration instance.</returns>
        public static FrostAuraApplicationConfiguration AddComponentCatelogNavItems(this FrostAuraApplicationConfiguration configuration)
        {
            var componentNavItems = configuration
                .GetType()
                .Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.BaseType.IsGenericType)
                .Where(t => t.BaseType.GetGenericTypeDefinition() == typeof(BaseComponent<object>).GetGenericTypeDefinition())
                .OrderBy(t => t.Name)
                .Select(t => new NavLink
                {
                    IconCssClass = "fa fa-cube",
                    Title = t.Name,
                    Path = $"/component/{t.FullName}"
                });

            configuration.NavigationItems.AddRange(componentNavItems);

            return configuration;
        }
    }
}
