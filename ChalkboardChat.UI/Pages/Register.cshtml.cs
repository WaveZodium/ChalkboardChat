using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ChalkboardChat.UI.Pages
{
    // En PageModel hanterar logiken för varje Razor Page
    // En PageModel motsvarar en Controller i MVC-mönstret och ViewModel i MVVM-mönstret
    public class RegisterModel : PageModel
    {
        // DEL 1 - MANAGERS
        // Skapa relation till UserManager för att spara och hantera användare 
        private readonly UserManager<IdentityUser> _userManager;
        // Skapa relation till SignInManager för att logga in användare
        // ... ??? och hålla koll på vilken användare som är inloggad (GÖR DEN DET?) ??? 
        private readonly SignInManager<IdentityUser> _signInManager; 
        
        // Konstruktor för modellen, tar managers som parametrar  
        public RegisterModel (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // DEL 2 - REGISTRERINGSMETODER 
        // Ta emot data från formuläret: EGENSKAPER
        [BindProperty, Required]
        public string UserName { get; set; }
        [BindProperty, Required]
        public string Email { get; set; }
        [BindProperty, Required]
        public string Password { get; set; }
        [BindProperty, Compare(nameof(Password), ErrorMessage = "Passwords do not match.")] // Inbyggd funktion för att jämföra lösenorden 
        public string ConfirmPassword { get; set; }

        // Ta emot data från formuläret: METOD för att SKAPA och SPARA användare 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // Om validering misslyckas
            {
                // -> visa formuläret igen med felmeddelanden
                return Page();
            }
            // Annars: skapa en ny användare med lokala variablen "user" 
            var user = new IdentityUser { UserName = UserName, Email = Email }; // Ej lösenord här! 

            // ...och sparar användaren i databasen med hjälp av lokala variabeln "result" 
            var result = await _userManager.CreateAsync(user, Password); // Här skickar vi med lösenord
            if (result.Succeeded) // Om användaren sparas i databasen
            {
                // -> logga in användaren och omdirigera till startsidan (sidan med meddelanden i detta fall)
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Member/Board");
            }
            else
            {
                // Om användaren inte sparas i databasen, visa felmeddelanden 
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                //... och visa formuläret igen 
                return Page();
            }
        }
    }
}