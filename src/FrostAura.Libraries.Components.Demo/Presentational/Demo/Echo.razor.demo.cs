using System;
using FrostAura.Libraries.Components.Shared.Abstractions;

namespace FrostAura.Libraries.Components.Demo.Presentational.Demo
{
    /// <summary>
    /// A demo component that simply renders an echo of what's passed to it, to the UI.
    /// </summary>
    public partial class Echo : BaseComponent<Echo>
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!EnableDemoMode) return;

            Text = "Hello World!";
        }
    }
}
