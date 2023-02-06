using Microsoft.AspNetCore.Components.Routing;
using System.Diagnostics;

namespace FrostAura.Libraries.Components.Models.Configuration
{
    /// <summary>
    /// FrostAura application configuration model.
    /// </summary>
    [DebuggerDisplay("{AppName} ({AppIdentity}) - {IdentityServerUrl}")]
    public class FrostAuraApplicationConfiguration
    {
        /// <summary>
        /// FrostAura identity server base url.
        /// </summary>
        public string IdentityServerUrl { get; set; } = "https://id.frostaura.net";
        /// <summary>
        /// FrostAura application identity.
        /// </summary>
        public string AppIdentity { get; set; }
        /// <summary>
        /// User-friendly application name.
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// Application icon in SVG format.
        /// </summary>
        public string AppIconSvgUrl { get; set; }
        /// <summary>
        /// FrostAura identity application secret.
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// Collection of scopes to request from the identity server.
        /// </summary>
        public List<string> Scopes { get; set; } = new List<string>();
        /// <summary>
        /// Collection of application navigation items.
        /// </summary>
        public List<NavLink> NavigationItems { get; set; } = new List<NavLink>();
        /// <summary>
        /// Google Maps API key.
        /// </summary>
        public string GoogleMapsApiKey { get; set; }
    }
}
