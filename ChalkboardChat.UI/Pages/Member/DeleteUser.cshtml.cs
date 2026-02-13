using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace ChalkboardChat.UI.Pages.Member
{
    public class DeleteUserModel : PageModel
    {
        // DEL 1 - MANAGERS 
        // Skapa relation till UserManager för att hantera inloggad användare
        private readonly UserManager<IdentityUser> _userManager;

        // Behöver skapa relation med meddelandedatabasen??? 
        private readonly AppDbContext _appDb;

        // KONSTRUKTOR för modellen, tar managers som parametrar
        public DeleteUserModel(UserManager<IdentityUser> userManager, AppDbContext appDb)
        {
            _userManager = userManager;
            _appDb = appDb; 
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
            // Annars: hitta användarens meddelanden 
            var username = user.UserName; 
            var messages = _appDb.Messages.Where(m => m.UserName == username); 
            // ...och ersätt användarens namn i hens meddelanden 
            foreach (var message in messages)
            {
                message.UserName = $"{username} (Elvis left the building)";
            }
            // ... och spara namnändringen i databasen
            await _appDb.SaveChangesAsync();
            // ...och ta bort användaren från databasen
            await _userManager.DeleteAsync(user); 
            // ...och redirekta till inloggningssidan
            return RedirectToPage("/Login");
        }
    }
}
