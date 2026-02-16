using ChalkboardChat.BLL.Interfaces;
using ChalkboardChat.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChalkboardChat.UI.Pages.Member
{
    // En PageModel hanterar logiken för varje Razor Page (motsvarar en Controller i MVC-mönstret och ViewModel i MVVM-mönstret)
    [Authorize] // NÖDVÄNDIGT för att skydda sidan så att endast inloggade användare kan se den
    public class UserInfoModel : PageModel
    {
        // DEL 1 - MANAGERS
        // Skapa relation till UserManager för att spara användare
        private readonly UserManager<IdentityUser> _userManager;
        // Skapa relation till MessengerService för att logga in användare och hålla koll på vilken användare som är inloggad (GÖR DEN DET?)

        private readonly IMessageService _iMessengerService;
        // Konstruktor för modellen, tar managers som parametrar

        public UserInfoModel(UserManager<IdentityUser> userManager, IMessageService iMessengerService)
        {
            _userManager = userManager;
            _iMessengerService = iMessengerService;
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

        // METOD som hämtar och visar användarens uppgifter
        public async Task<IActionResult> OnGetAsync()
        {
            // Hämta den inloggade användarens uppgifter
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Login");
            }
            // Visa användarens befintliga uppgifter i formuläret
            NewUserName = user.UserName;
            NewEmail = user.Email;
            // NewPassword lämnas tomt - lösenord ska inte visas!
            
            return Page();
        }

        // METOD för att ta emot och spara/uppdatera eventuellt ändrad data från formuläret
        public async Task<IActionResult> OnPostAsync()
        {
            // Kollar att användaren är inloggad och validerar inmatningar
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Hämtar användarens uppgifter
            var user = await _userManager.GetUserAsync(User);
            // Om hen inte hittas - gå vidare till login
            if (user == null)
            {
                return RedirectToPage("/Login");
            }
            var oldUsername = user.UserName;
            // Annars: Tilldela nya uppgifter
            user.UserName = NewUserName;
            user.Email = NewEmail;
            // Spara ändringarna i databasen
            var result = await _userManager.UpdateAsync(user);
            // Om lyckas
            if (result.Succeeded)
            {
                // Uppdatera namn i meddelanden
                await _iMessengerService.UpdateUsernameInMessagesAsync(user.Id, NewUserName);
                return RedirectToPage("/Start");
            }
            // Om uppgifterna inte sparas, skicka felmeddelanden 
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            // ... och visa formuläret igen 
            return Page();
        }
    }
}
