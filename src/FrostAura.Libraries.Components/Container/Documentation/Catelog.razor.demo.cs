using System;
using FrostAura.Libraries.Components.Shared.Abstractions;

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
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!EnableDemoMode) return;

            ComponentsAssembly = typeof(FrostAura
                .Libraries
                .Components
                .Demo
                .Presentational
                .Demo
                .Echo).Assembly;
            // Wire up some hacky internal routing.
            OnComponentSelected = (componentName) =>
            {
                FocusedComponentName = componentName;
                OnParametersSet();
                StateHasChanged();
            };
        }
    }
}
