using System.ComponentModel.DataAnnotations;

namespace Baldly.Data.ViewModels;

public class LoginVm
{
    [Required(ErrorMessage = "Email address is required")]
    [CustomEmailValidator(ErrorMessage = "Email address is not valid (custom)")]
    public string? EmailAddress { get; set; }


    [Required(ErrorMessage = "Password is required")]
    [MinLength(5, ErrorMessage = "Password must be at least 5 characters")]
    public string? Password { get; set; }
}