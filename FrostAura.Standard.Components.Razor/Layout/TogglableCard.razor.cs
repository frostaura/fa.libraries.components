using FrostAura.Standard.Components.Razor.Abstractions;
using Microsoft.AspNetCore.Components;

namespace FrostAura.Standard.Components.Razor.Layout
{
    /// <summary>
    /// Component to provide a card that's content can expand and collapse.
    /// </summary>
    public partial class TogglableCard : BaseComponent<TogglableCard>
    {
        /// <summary>
        /// Head content to display which is always visible.
        /// </summary>
        [Parameter]
        public RenderFragment<TogglableCard> HeaderContent { get; set; }
        /// <summary>
        /// Body content to display when collap
        /// </summary>
        [Parameter]
        public RenderFragment BodyContent { get; set; }
        /// <summary>
        /// Whether the body is currently invisible.
        /// </summary>
        [Parameter]
        public bool Expanded { get; set; }

        /// <summary>
        /// Toggle whether content should be shown.
        /// </summary>
        public void ToggleExpanded()
        {
            Expanded = !Expanded;
            StateHasChanged();
        }
    }
}
