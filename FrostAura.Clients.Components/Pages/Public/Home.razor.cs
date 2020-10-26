using FrostAura.Standard.Components.Razor.Abstractions;
using System.Threading.Tasks;

namespace FrostAura.Clients.Components.Pages.Public
{
    /// <summary>
    /// Public home / anonymous auth component.
    /// </summary>
    public partial class Home : BaseComponent<Home>
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            NavigationService.PageTitleStream.Value = "Public";
        }
    }
}
