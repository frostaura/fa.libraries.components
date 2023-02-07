using System;
using FrostAura.Libraries.Components.Shared.Abstractions;

namespace FrostAura.Libraries.Components.Presentational.Status
{
    /// <summary>
    /// Component to display a decreasing bar after which an event fires.
    /// </summary>
    public partial class CountdownBar : BaseComponent<CountdownBar>, IDisposable
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (!EnableDemoMode) return;

            Infinite = true;
            Duration = TimeSpan.FromSeconds(15);
        }
    }
}
