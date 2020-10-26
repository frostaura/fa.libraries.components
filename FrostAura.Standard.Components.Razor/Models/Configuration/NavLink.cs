using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;

namespace FrostAura.Standard.Components.Razor.Models.Configuration
{
    /// <summary>
    /// Navigational link configuration model.
    /// </summary>
    [DebuggerDisplay("Title: {Title}, CSS: {CssClass}")]
    public class NavLink
    {
        /// <summary>
        /// Display title of the navigation item.
        /// </summary>
        public string Title { get; set; } = "UNSET_TITLE";
        /// <summary>
        /// CSS class to apply for the icon portion of the navigation item.
        /// </summary>
        public string IconCssClass { get; set; } = string.Empty;
        /// <summary>
        /// What kind of match type to configure the router with.
        /// </summary>
        public NavLinkMatch MatchType { get; set; } = NavLinkMatch.Prefix;
        /// <summary>
        /// Path which to navigate to.
        /// </summary>
        public string Path { get; set; } = "/";
        /// <summary>
        /// Whether this nav item should be rendered only when the user is authenticated.
        /// </summary>
        public bool RequireAuthentication { get; set; }
    }
}
