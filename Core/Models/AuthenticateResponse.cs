
using BaseDotnet.Core.Entities;

namespace BaseDotnet.Core.Models;

using BaseDotnet.Modules.User;
using BaseDotnet.Modules.Role;

public class AuthenticateResponse
{
    public int UserID { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string DisplayName { get; set; }
    public string Status { get; set; }
    public DateTime? LastLogin { get; set; }

    public Role? role { get; set; }

    public AuthenticateResponse(User user, string token)
    {
        UserID = user.UserID;
        Email = user.Email;
        LastLogin = user.LastLogin;
        Username = user.UserName;
        Token = token;
        Status = user.Status;
        DisplayName = user.DisplayName;
    }
}