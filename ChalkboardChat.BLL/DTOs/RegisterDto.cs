using System.ComponentModel.DataAnnotations;

namespace MessageBoardApp.DTOs;


public class RegisterDto
{
    [Required(ErrorMessage = "Användarnamn krävs")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Användarnamnet måste vara mellan 3 och 50 tecken")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lösenord krävs")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara minst 6 tecken")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bekräfta lösenord")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
