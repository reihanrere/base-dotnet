using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseDotnet.Modules.Users.Models;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Display name is required")]
    public string DisplayName { get; set; }

    // Optional dengan default value
    [DefaultValue("Active")]
    public string Status { get; set; } = "Active";

    // Optional dengan default value
    [DefaultValue(3)] // Default ke role "User"
    public int RoleID { get; set; } = 3;
}