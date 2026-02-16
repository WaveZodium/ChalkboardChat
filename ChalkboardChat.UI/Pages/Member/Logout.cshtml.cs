using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChalkboardChat.UI.Pages.Member
{
    [Authorize] // endast inloggade användare kan logga ut
    public class LogoutModel : PageModel
    {
        // skapa relation till SignInManager för att hantera utloggning
        private readonly SignInManager<IdentityUser> _signInManager;

        // konstruktor för modellen
        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // metod som körs vid post-request (när användaren klickar på logga ut)
        public async Task<IActionResult> OnPostAsync()
        {
            // logga ut användaren
            await _signInManager.SignOutAsync();
            // redirecta till startsidan eller inloggningssidan
            return RedirectToPage("/Index");
        }

        // metod som körs vid get-request (om någon navigerar direkt till sidan)
        public async Task<IActionResult> OnGetAsync()
        {
            // logga ut användaren
            await _signInManager.SignOutAsync();
            // redirecta till startsidan eller inloggningssidan
            return RedirectToPage("/Index");
        }
    }
}
