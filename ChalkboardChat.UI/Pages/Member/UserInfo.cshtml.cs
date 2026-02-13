using ChalkboardChat.UI.MockComponents;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChalkboardChat.UI.Pages.Member
{
    public class UserInfoModel : PageModel
    {
        // DEL 1 - MANAGERS
        // Skapa relation till UserManager för att spara användare
        private readonly UserManager<IdentityUser> _userManager;
        // Skapa relation till SignInManager för att logga in användare och hålla koll på vilken användare som är inloggad (GÖR DEN DET?)

        //private readonly SignInManager<IdentityUser> _signInManager;
        // Konstruktor för modellen, tar managers som parametrar

        // Skapar relation med mockmeddelandedatabasen för testning
        private readonly MockAppDbContext _appDb;

        public UserInfoModel(UserManager<IdentityUser> userManager, MockAppDbContext appDb)
        {
            _userManager = userManager;
            _appDb = appDb;
        }
        // DEL 2 - REGISTRERINGSMETODER 
        // Ta emot data från formuläret: EGENSKAPER
        [BindProperty, Required]
        public string NewUserName { get; set; }
        [BindProperty, Required]
        public string NewEmail { get; set; }
        [BindProperty, Required]
        public string NewPassword { get; set; }
        [BindProperty, Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")] // Inbyggd funktion för att jämföra lösenorden 
        public string ConfirmNewPassword { get; set; }

        //// METOD som hämtar och visar användarens uppgifter
        //public async Task<IActionResult> OnGetAsync()
        //{
        //    // Hämtar den inloggade användarens uppgifter
        //    var user = await _userManager.GetUserAsync(User);
        //    // Om hen inte hittas - gå vidare till login
        //    if (user == null)
        //    {
        //        return RedirectToPage("/Login");
        //    }
        //    // Visa användarens befintliga uppgifter i formuläret
        //    var currentUserName = user.UserName;
        //    var currentEmail = user.Email;
        //    // Password lämnas tomt - lösenord ska inte visas!
        //    // Visa sidan (som laddas om) 
        //    return Page();
        //}

        // METOD för att visa befintliga och sedan ta emot och spara/uppdatera eventuellt ändrad data
        public async Task<IActionResult> OnPostAsync()
        {
           // Hämtar användarens uppgifter
           var user = await _userManager.GetUserAsync(User);
            // Om hen inte hittas - gå vidare till login
            if (user == null)
            {
                return RedirectToPage("/Index");
            }
            // Annars: visa användarens befintliga uppgifter
            var currentUserName = user.UserName;
            var currentEmail = user.Email;  // OBS: Password lämnas tomt - lösenord ska inte visas!
            // ...validera inmatningar
            if (!ModelState.IsValid)
            {
                // Om inte - gå tillbaka till befintliga uppgifter
                return Page();
            }
            // Annars: tilldela nya uppgifter
            user.UserName = NewUserName;
            user.Email = NewEmail;
            // Spara ändringarna i databasen
            var result = await _userManager.UpdateAsync(user);
            // Om inte lyckas
            if (!result.Succeeded)
            {
                // Visa in sidan igen 
                return Page();
            }
            // Annars: Uppdatera namn i meddelanden 
            var messages = _appDb.Messages.Where(m => m.UserName == NewUserName);
            // ...och ladda om sidan för att visa ändringarna
            return RedirectToPage("/Start");
        }
    }
}
