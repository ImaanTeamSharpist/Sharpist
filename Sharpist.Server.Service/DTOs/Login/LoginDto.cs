using System.ComponentModel.DataAnnotations;

namespace Sharpist.Server.Service.DTOs.Login;


public class LoginDto
{
    [Required(ErrorMessage = "Email'ni kiriting"), EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Parolni kiriting")]
    public string Password { get; set; }
}
