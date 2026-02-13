using ChalkboardChat.UI.Context;
using ChalkboardChat.UI.MockComponents;
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

        // Behöver skapa relation med användardatabasen 
        private readonly AuthDbContext _authDb;
        // Skapa relation till MessageService för att hämta meddelanden från databasen
        private readonly MockMessageService _messageService;

        // KONSTRUKTOR för modellen, tar manager som parametrar
        public DeleteUserModel(UserManager<IdentityUser> userManager, AuthDbContext authDb, MockMessageService messageService)
        {
            _userManager = userManager;
            _messageService = messageService;
            _authDb = authDb;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Hämta den inloggade användarens uppgifter
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Om användare inte hittas, skicka vidare till inloggningssidan
                return RedirectToPage("/Index");
            }
            // Annars: hitta användarens meddelanden 
            var username = user.UserName; 
            var messages = _messageService.GetAllMessagesAsync().Result.Where(m => m.UserName == username); 
            // ...och ersätt användarens namn i hens meddelanden 
            foreach (var message in messages)
            {
                message.UserName = $"{username} (Elvis left the building)";
            }
            // ... och spara namnändringen i databasen
            await _authDb.SaveChangesAsync();
            // ...och ta bort användaren från databasen
            await _userManager.DeleteAsync(user);
            // ...och redirekta till inloggningssidan
            return RedirectToPage("/Index");
        }
    }
}
