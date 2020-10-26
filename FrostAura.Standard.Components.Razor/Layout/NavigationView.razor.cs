using FrostAura.Standard.Components.Razor.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FrostAura.Standard.Components.Razor.Layout
{
    /// <summary>
    /// Component for displaying navigational view content.
    /// </summary>
    public partial class NavigationView : BaseComponent<NavigationView>
    {
        /// <summary>
        /// Content to be displayed inside of the view wrapper.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
