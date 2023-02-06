using FrostAura.Libraries.Components.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace FrostAura.Libraries.Components.Presentational.Content
{
    /// <summary>
    /// Toggle between two templates as async work gets done.
    /// </summary>
    public partial class AsyncLoader : BaseComponent<AsyncLoader>
    {
        /// <summary>
        /// The current version of the component.
        /// </summary>
        public override Version Version { get; } = new Version(1, 0, 0);
        /// <summary>
        /// Content to display during loading.
        /// </summary>
        [Parameter]
        public RenderFragment LoadingContent { get; set; }
        /// <summary>
        /// Content to display after loading.
        /// </summary>
        [Parameter]
        public RenderFragment FinalContent { get; set; }
        /// <summary>
        /// Async work to complete.
        /// </summary>
        [Parameter]
        public Task AsyncWork { get; set; }
        /// <summary>
        /// Whether the component is currently busy.
        /// </summary>
        private bool _isLoading { get; set; } = true;

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                if (AsyncWork != null) await AsyncWork;
            }
            catch (Exception e)
            {
                Logger?.LogError($"Failed to await the loader task: '{e.Message}'");
            }
            finally
            {
                _isLoading = false;

                StateHasChanged();
            }
        }
    }
}
