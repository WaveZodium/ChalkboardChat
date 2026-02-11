using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChalkboardChat.DAL.Models;
using ChalkboardChat.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChalkboardChat.UI.Pages.Member
{
    // En PageModel hanterar logiken för varje Razor Page
    // En PageModel motsvarar en Controller i MVC-mönstret och ViewModel i MVVM-mönstret
    public class StartModel : PageModel
    {
        // DEL 1 - MANAGERS och SERVICES 
        // Skapa relation till SignInManager för att hålla koll på vilken användare som är inloggad (GÖR DEN DET?) 
        private readonly SignInManager<IdentityUser> _signInManager;
        // Skapa relation till MessageService för att hämta meddelanden från databasen
        private readonly MessageService _messageService;

        // KONSTRUKTOR för modellen, tar managers och services som parametrar  
        public StartModel(SignInManager<IdentityUser> signInManager, MessageService messageService)
        {
            _signInManager = signInManager;
            _messageService = messageService;
        }

        // EGENSKAP för att hålla listan med meddelanden
        public List<MessageModel> Messages { get; set; }

        // METOD för att visa hela listan med meddelanden 
        public async Task OnGetAsync()
        {
            // Hämta alla meddelanden från databasen och spara i lokal variabel "Messages"
            Messages = await _messageService.GetAllMessagesAsync();
        }
        // METOD för att skicka nytt meddelande till databas
        public async Task<IActionResult> OnPostAsync(string message)
        {
            // Kolla så att det finns innehåll
            if (!string.IsNullOrWhiteSpace(message))
            {
                // Spara det nya meddelandet i databasen
                await _messageService.AddMessageAsync(message, 
                    User.Identity!.Name);
            }
            // ... och omdirigera till sida för att visa meddelandelistan igen
            return RedirectToPage(); 
        }
    }
}
