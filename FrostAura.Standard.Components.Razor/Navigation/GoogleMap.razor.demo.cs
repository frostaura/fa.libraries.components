using FrostAura.Standard.Components.Razor.Abstractions;
using FrostAura.Standard.Components.Razor.Models.Geolocation;

namespace FrostAura.Standard.Components.Razor.Navigation
{
    /// <summary>
    /// Google map component.
    /// </summary>
    public partial class GoogleMap : BaseComponent<GoogleMap>
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (!EnableDemoMode) return;

            ApiKey = Configuration.GoogleMapsApiKey;
            Center = new GeoPoint
            {
                Latitude = "-29.837145",
                Longitude = "24.521990"
            };
            Zoom = 14;
            MapType = Enums.GoogleMap.MapType.Satellite;
        }
    }
}
