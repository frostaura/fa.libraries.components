using System;
using FrostAura.Libraries.Components.Abstractions;

namespace FrostAura.Libraries.Components.Container.Documentation
{
    /// <summary>
    /// Toggle between two templates as async work gets done.
    /// </summary>
    public partial class Catelog : BaseComponent<Catelog>
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override Task OnInitializedAsync()
        {
            base.OnInitialized();

            if (!EnableDemoMode) return Task.CompletedTask;

            return Task.CompletedTask;
        }
    }
}
