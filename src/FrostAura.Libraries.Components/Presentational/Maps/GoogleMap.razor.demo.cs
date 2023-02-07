using System;
using FrostAura.Libraries.Components.Abstractions;

namespace FrostAura.Libraries.Components.Presentational.Maps
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

            Center = new Shared.Models.Map.GeoPoint
            {
                Latitude = "40.712776",
                Longitude = "-74.005974"
            };
        }
    }
}
