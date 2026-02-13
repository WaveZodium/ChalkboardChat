using System.ComponentModel.DataAnnotations;

namespace MessageBoardApp.DTOs;

// hämta användare
public class UpdateUsernameDto
{
    [Required(ErrorMessage = "Användarnamn krävs")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Användarnamnet måste vara mellan 3 och 50 tecken")]
    public string NewUsername { get; set; } = string.Empty;
}

//ändra lösenord
public class UpdatePasswordDto
{
    [Required(ErrorMessage = "Nuvarande lösenord krävs")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nytt lösenord krävs")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara minst 6 tecken")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bekräfta lösenord")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

//ta bort konto
public class DeleteAccountDto
{
    [Required(ErrorMessage = "Lösenord krävs för att bekräfta radering")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
