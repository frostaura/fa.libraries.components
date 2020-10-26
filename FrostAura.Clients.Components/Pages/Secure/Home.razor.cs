using FrostAura.Standard.Components.Razor.Abstractions;
using System.Threading.Tasks;

namespace FrostAura.Clients.Components.Pages.Secure
{
    /// <summary>
    /// Settings component for authenticated users.
    /// </summary>
    public partial class Home : BaseAuthenticatedComponent<Home>
    {
        /// <summary>
        /// Lifecycle event.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            NavigationService.PageTitleStream.Value = "Secure Home";
        }
    }
}
