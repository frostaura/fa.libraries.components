using Microsoft.AspNetCore.Builder;

namespace FrostAura.Standard.Components.Razor.Extensions
{
    /// <summary>
    /// Application builder extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use FrostAura components.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder UseFrostAuraComponents(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
