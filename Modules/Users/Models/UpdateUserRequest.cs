using System.ComponentModel.DataAnnotations;

namespace BaseDotnet.Modules.Users.Models;

public class UpdateUserRequest
{
    [Required(ErrorMessage = "User ID is required")]
    public int UserID { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    // Password optional saat update (jika null = tidak diubah)
    public string? Password { get; set; }

    [Required(ErrorMessage = "Display name is required")]
    public string DisplayName { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public string Status { get; set; }

    [Required(ErrorMessage = "Role ID is required")]
    public int RoleID { get; set; }
}