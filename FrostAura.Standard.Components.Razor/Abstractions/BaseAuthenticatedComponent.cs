using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace FrostAura.Standard.Components.Razor.Abstractions
{
    /// <summary>
    /// FrostAura base component for core and shared functionality.
    /// </summary>
    public abstract class BaseAuthenticatedComponent<TComponentType> : BaseComponent<TComponentType>
    {
        /// <summary>
        /// FrostAura navigation service.
        /// </summary>
        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        /// <summary>
        /// Authenticated state of the application.
        /// </summary>
        [CascadingParameter]
        protected Task<AuthenticationState> AuthenthicationState { get; set; }
        /// <summary>
        /// Auth token.
        /// </summary>
        protected string Token { get; private set; }

        /// <summary>
        /// Ensure the token is valid after render.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (HttpContextAccessor?.HttpContext == null) return;

            try
            {
                var token = await GetAuthTokenAsync();
                var parsedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var hasTokenExpired = DateTime.UtcNow > parsedToken.ValidTo;

                if (hasTokenExpired)
                {
                    Logger.LogWarning("Token has expired. Redirecting to the logout page. Consider implementing refresh tokens.");

                    var currentUri = NavigationManager.Uri;

                    Token = null;

                    NavigationService.NavigateClientTo($"identity/logout?redirectUri={Uri.EscapeUriString(currentUri)}");
                }
                else
                {
                    Token = token;
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Failed to validate auth token. Redirecting to login page: '{e.Message}'");
            }
        }

        /// <summary>
        /// Retrieve the currently authenticated token.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAuthTokenAsync()
        {
            var accessToken = await HttpContextAccessor.HttpContext.GetTokenAsync("access_token");

            if (string.IsNullOrWhiteSpace(accessToken)) Logger.LogWarning("Failed to extract 'access_token' from the http context.");

            return accessToken;
        }
    }
}
