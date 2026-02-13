using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ChalkboardChat.UI.Pages
{
    // En PageModel hanterar logiken för varje Razor Page
    // En PageModel motsvarar en Controller i MVC-mönstret och ViewModel i MVVM-mönstret
    public class LoginModel : PageModel
    {
        // DEL 1 - MANAGER
        // Skapa relation till SignInManager för att logga in användare 
        private readonly SignInManager<IdentityUser> _signInManager;

        // Konstruktor för modellen, tar managers som parametrar  
        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // DEL 2 - REGISTRERINGSMETODER 
        // Ta emot data från formuläret: EGENSKAPER
        [BindProperty, Required]
        public string UserName { get; set; }
        [BindProperty, Required]
        public string Password { get; set; }

        // Ta emot data från formuläret: METOD för att TA EMOT data från användare 
        public async Task<IActionResult> OnPostAsync()
        {
            // Validera inmatningar och spara utfallet i lokal variabel "result" 
            var result = await _signInManager.PasswordSignInAsync(UserName, Password, isPersistent: false, lockoutOnFailure: false);
            // Om inloggningen lyckas
            if (result.Succeeded)
            {
                // Omdirigera till startsidan (med meddelanden) 
                return RedirectToPage("/Login");
            }
            // Om inloggningen misslyckas
            else
            {
                // Lägg till felmeddelande och visa formuläret igen
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }
    }
}
