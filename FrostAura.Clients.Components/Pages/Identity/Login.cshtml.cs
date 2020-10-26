using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FrostAura.Clients.Components.Pages.Identity
{
    /// <summary>
    /// Endpoint handler for logging in.
    /// </summary>
    public class LoginModel : PageModel
    {
        /// <summary>
        /// On fetching this page.
        /// </summary>
        public async Task OnGetAsync(string redirectUri)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var idToken = await HttpContext.GetTokenAsync("id_token");
                var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

                var claims = HttpContext.User.Claims.ToList();
                var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

                Response.Redirect(redirectUri ?? "/");
            }
            else
            {
                await HttpContext.ChallengeAsync("oidc", new AuthenticationProperties
                {
                    RedirectUri = redirectUri
                });
            }
        }
    }
}