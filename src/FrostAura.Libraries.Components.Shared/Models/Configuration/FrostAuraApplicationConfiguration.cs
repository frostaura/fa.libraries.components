using System.Diagnostics;

namespace FrostAura.Libraries.Components.Shared.Models.Configuration
{
    /// <summary>
    /// FrostAura general application configuration model.
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
        /// 
        /// Example: "fa.clients.pointskeeper"
        /// </summary>
        public string AppIdentity { get; set; }
        /// <summary>
        /// User-friendly application name.
        /// 
        /// Example: "PointsKeeper"
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// The base URL of the client-facing application.
        /// 
        /// Example: "https://pointskeeper.azurewebsites.net"
        /// </summary>
        public string AppBaseUrl { get; set; }
        /// <summary>
        /// Platform-specific maps API key.
        /// </summary>
        public string MapsApiKey { get; set; }
    }
}
