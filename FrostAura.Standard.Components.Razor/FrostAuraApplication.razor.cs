using FrostAura.Standard.Components.Razor.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FrostAura.Standard.Components.Razor
{
    /// <summary>
    /// Main FrostAura application wrapper.
    /// </summary>
    /// <typeparam name="TProgramType">Type of the calling applciation.</typeparam>
    public partial class FrostAuraApplication<TProgramType> : BaseComponent<FrostAuraApplication<TProgramType>>
    {
        /// <summary>
        /// JavaScript runtime engine.
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        /// <param name="firstRender">Whether the component is rendering for the first time.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender) return;

            await WireUpClientDefaultReconnectHandlerAsync();
        }

        /// <summary>
        /// Wire up a custom default reconnect handler for Blazor on the client.
        /// </summary>
        private async Task WireUpClientDefaultReconnectHandlerAsync()
        {
            var jsString = @"
                Blazor.defaultReconnectionHandler._reconnectCallback = function(d) {
                    document.location.reload();
                }
            ";

            await JsRuntime.InvokeVoidAsync("eval", jsString);
        }
    }
}
