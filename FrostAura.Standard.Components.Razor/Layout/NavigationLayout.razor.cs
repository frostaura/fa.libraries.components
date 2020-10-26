using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FrostAura.Standard.Components.Razor.Layout
{
    /// <summary>
    /// Component for encaptulating the navigation for an app.
    /// </summary>
    public partial class NavigationLayout : LayoutComponentBase
    {
        /// <summary>
        /// JS runtime engine.
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Lifecycle event.
        /// </summary>
        /// <param name="firstRender">Whether the app is rendering for the first time.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await JsRuntime.InvokeVoidAsync("resizeNavigationLayout");
        }
    }
}
