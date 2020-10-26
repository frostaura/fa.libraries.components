using FrostAura.Standard.Components.Razor.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace FrostAura.Standard.Components.Razor.Layout
{
    /// <summary>
    /// Component to visualize the navigation menu.
    /// </summary>
    public partial class NavigationSidebar : BaseAuthenticatedComponent<NavigationSidebar>
    {
        /// <summary>
        /// Runtime to allow for JS execution.
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        /// <summary>
        /// Internal transform property to use as the CSS class for the nav menu.
        /// </summary>
        protected string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;
        /// <summary>
        /// Navigation bar page title.
        /// </summary>
        protected string PageTitle = string.Empty;
        /// <summary>
        /// Whether the menu is currently collapsed.
        /// </summary>
        private bool _collapseNavMenu = true;

        /// <summary>
        /// Upon parameters set, bootstrap the component.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            NavigationService
                .PageTitleStream
                .Subscribe(val =>
                {
                    PageTitle = val;
                    JsRuntime.InvokeVoidAsync("setPageTitle", $"{Configuration.AppName} - {val}");
                    StateHasChanged();
                });
        }

        /// <summary>
        /// Navigate to the home page.
        /// </summary>
        protected void NavigateHome()
        {
            NavigationManager.NavigateTo("/");
        }

        /// <summary>
        /// Toggle whether to show the nav menu.
        /// </summary>
        protected void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }
    }
}
