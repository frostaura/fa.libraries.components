using FrostAura.Libraries.Components.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FrostAura.Libraries.Components.Presentational.Status
{
    /// <summary>
    /// Component to display a decreasing bar after which an event fires.
    /// </summary>
    public partial class CountdownBar : BaseComponent<CountdownBar>, IDisposable
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(1, 0, 0);
        /// <summary>
        /// Duration of the countdown. 
        /// </summary>
        [Parameter]
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(1);
        /// <summary>
        /// Event to fire when countdown reaches zero.
        /// </summary>
        [Parameter]
        public Action OnCountdownZero { get; set; }
        /// <summary>
        /// Whether the countdown task should run indefinitely.
        /// </summary>
        [Parameter]
        public bool Infinite { get; set; }
        /// <summary>
        /// Cancellation token source to indicate a stop when the countdown bar is on repeat.
        /// </summary>
        [Parameter]
        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

        /// <summary>
        /// Cleanup task for the component.
        /// </summary>
        public void Dispose()
        {
            CancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        /// <param name="firstRender">Whether this was the component's first render.</param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender) return;

            await InitiateCountdownAsync();
        }

        /// <summary>
        /// Initiate the countdown.
        /// </summary>
        /// <returns></returns>
        private async Task InitiateCountdownAsync()
        {
            if (CancellationTokenSource.Token.IsCancellationRequested) return;

            await Task.Delay(Duration);
            OnCountdownZero?.Invoke();

            if (Infinite) await InitiateCountdownAsync();
        }
    }
}

