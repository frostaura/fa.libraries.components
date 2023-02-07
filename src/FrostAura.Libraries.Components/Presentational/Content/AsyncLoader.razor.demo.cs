using System;
using FrostAura.Libraries.Components.Abstractions;
using FrostAura.Libraries.Components.Presentational.Input;
using FrostAura.Libraries.Components.Shared.Enums.Input;
using FrostAura.Libraries.Components.Shared.Models.Input;
using FrostAura.Libraries.Data.Models.EntityFramework;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FrostAura.Libraries.Components.Presentational.Content
{
    /// <summary>
    /// Toggle between two templates as async work gets done.
    /// </summary>
    public partial class AsyncLoader : BaseComponent<AsyncLoader>
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (!EnableDemoMode) return;

            var secondsToSimulateLoading = 5;

            LoadingContent = builder =>
            {
                var sequence = 0;

                builder.OpenElement(sequence++, "div");
                builder.AddContent(sequence++, $"Artificially loading for {secondsToSimulateLoading} seconds, please wait...");
                builder.CloseElement();
            };
            FinalContent = builder =>
            {
                var sequence = 0;

                builder.OpenElement(sequence++, "div");
                builder.AddContent(sequence++, "All done loading!");
                builder.CloseElement();
            };
            AsyncWork = Task.Delay(TimeSpan.FromSeconds(secondsToSimulateLoading));
        }
    }
}
