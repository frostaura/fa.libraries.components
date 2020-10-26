using FrostAura.Libraries.Core.Extensions.Reactive;
using FrostAura.Libraries.Core.Interfaces.Reactive;
using Microsoft.JSInterop;
using FrostAura.Libraries.Core.Extensions.Validation;
using FrostAura.Standard.Components.Razor.Interfaces.Navigation;

namespace FrostAura.Standard.Components.Razor.Services.Navigation
{
    /// <summary>
    /// Navigation service for Blazor pages.
    /// </summary>
    public class PageNavigationService : INavigationService
    {
        /// <summary>
        /// Subscribable event steam for the page title.
        /// 
        /// Default value: string.Empty.
        /// </summary>
        public IObservedValue<string> PageTitleStream { get; } = string.Empty.AsObservedValue();
        /// <summary>
        /// JS runtime engine.
        /// </summary>
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Provide dependencies.
        /// </summary>
        /// <param name="jsRuntime">JS runtime engine.</param>
        public PageNavigationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime
                .ThrowIfNull(nameof(jsRuntime));
        }

        /// <summary>
        /// Navigate on the client-side to a specified URL.
        /// </summary>
        /// <param name="url">URL to navigate to on the client-side.</param>
        public void NavigateClientTo(string url)
        {
            _jsRuntime.InvokeVoidAsync("navigateTo", url);
        }
    }
}
