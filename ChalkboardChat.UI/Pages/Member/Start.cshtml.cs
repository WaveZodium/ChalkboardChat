using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChalkboardChat.DAL.Models;
using ChalkboardChat.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using ChalkboardChat.DAL;
using Microsoft.AspNetCore.Authorization;
using ChalkboardChat.BLL.Interfaces;
using ChalkboardChat.BLL.DTOs;

namespace ChalkboardChat.UI.Pages.Member
{
    // En PageModel hanterar logiken för varje Razor Page (motsvarar en Controller i MVC-mönstret och ViewModel i MVVM-mönstret)
    [Authorize] // NÖDVÄNDIGT för att skydda sidan så att endast inloggade användare kan se den
    public class StartModel : PageModel
    {
        // DEL 1 - MANAGERS och SERVICES 
        // Skapa relation till SignInManager för att hålla koll på vilken användare som är inloggad (GÖR DEN DET?) 
        private readonly SignInManager<IdentityUser> _signInManager;
        // Skapa relation till MessageService för att hämta meddelanden från databasen
        private readonly IMessageService _iMessageService;

        // KONSTRUKTOR för modellen, tar managers och services som parametrar  
        public StartModel(SignInManager<IdentityUser> signInManager, IMessageService iMessageService)
        {
            _signInManager = signInManager;
            _iMessageService = iMessageService;
        }

        // EGENSKAP för att hålla listan med meddelanden
        public List<MessageDto> Messages { get; set; }

        // METOD för att visa hela listan med meddelanden 
        public async Task OnGetAsync()
        {
            // hämta den inloggade användarens id
            var user = await _signInManager.UserManager.GetUserAsync(User);
            var currentUserId = user?.Id ?? string.Empty;

            // Hämta alla meddelanden från databasen och spara i lokal variabel "Messages"
            Messages = await _iMessageService.GetAllMessagesAsync(currentUserId);
        }
        // METOD för att skicka nytt meddelande till databas
        public async Task<IActionResult> OnPostAsync(string message)
        {
            // Kolla så att det finns innehåll
            if (!string.IsNullOrWhiteSpace(message))
            {
                // hämta den inloggade användarens id
                var user = await _signInManager.UserManager.GetUserAsync(User);

                // skapa dto för meddelande
                var dto = new CreateMessageDto
                {
                    Message = message,
                    UserId = user?.Id ?? string.Empty
                };

                // Spara det nya meddelandet i databasen
                await _iMessageService.PostMessageAsync(dto);
            }
            // ... och omdirigera till sida för att visa meddelandelistan igen
            return RedirectToPage(); 
        }
    }
}
