using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace FrostAura.Clients.Components.Pages.Identity
{
    /// <summary>
    /// Endpoint handler for logging out.
    /// </summary>
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// On fetching this page.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(string redirectUri)
        {
            await HttpContext.SignOutAsync();

            return Redirect(redirectUri ?? "/");
        }
    }
}