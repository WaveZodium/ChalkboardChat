using ChalkboardChat.BLL.Interfaces;
using ChalkboardChat.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace ChalkboardChat.UI.Pages.Member
{
    // En PageModel hanterar logiken för varje Razor Page (motsvarar en Controller i MVC-mönstret och ViewModel i MVVM-mönstret)
    [Authorize] // NÖDVÄNDIGT för att skydda sidan så att endast inloggade användare kan se den
    public class DeleteUserModel : PageModel
    {
        // DEL 1 - MANAGERS 
        // Skapa relation till UserManager för att hantera inloggad användare
        private readonly UserManager<IdentityUser> _userManager;
        // Skapa relation till messageservice för att hantera inloggad användare
        private readonly IMessageService _iMessengerService;
        // Konstruktor för modellen, tar managers som parametrar
        public DeleteUserModel(UserManager<IdentityUser> userManager, IMessageService iMessengerService)
        {
            _userManager = userManager;
            _iMessengerService = iMessengerService;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Hämta den inloggade användarens uppgifter
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Om användare inte hittas, skicka vidare till inloggningssidan
                return RedirectToPage("/Login");
            }
            // ta bort användaren från databasen
            await _userManager.DeleteAsync(user); 
            // ...och redirekta till inloggningssidan
            return RedirectToPage("/Login");
        }
    }
}
