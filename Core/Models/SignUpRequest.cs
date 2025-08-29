namespace BaseDotnet.Core.Models;

using System.ComponentModel.DataAnnotations;

public class SignUpRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string DisplayName { get; set; }

}