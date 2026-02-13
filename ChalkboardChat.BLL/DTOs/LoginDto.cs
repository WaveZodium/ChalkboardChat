using System.ComponentModel.DataAnnotations;

namespace MessageBoardApp.DTOs;


public class LoginDto
{
    [Required(ErrorMessage = "Användarnamn krävs")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lösenord krävs")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
