using FrostAura.Standard.Components.Razor.Abstractions;
using System;
using System.Threading.Tasks;

namespace FrostAura.Standard.Components.Razor.Security
{
    /// <summary>
    /// Component to automatically redirect the client to the sign in page when not authenticated.
    /// </summary>
    public partial class EnsureAuthenticatedUser : BaseAuthenticatedComponent<EnsureAuthenticatedUser>
    {
        /// <summary>
        /// Upon component initialization.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var user = (await AuthenthicationState).User;

            if (!user.Identity.IsAuthenticated)
            {
                var currentUri = NavigationManager.Uri;

                NavigationManager.NavigateTo($"identity/login?redirectUri={Uri.EscapeUriString(currentUri)}");
            }
        }
    }
}