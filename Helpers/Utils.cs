
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BaseDotnet.Entities;
namespace BaseDotnet.Helpers;

public class Utils
{
    public static string HashPassword(string password, byte[] salt)
    {
        using (var sha256 = new SHA256Managed())
        {
            byte[] combinedBytes = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            byte[] hashedBytes = sha256.ComputeHash(combinedBytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
    public static bool VerifyPassword(string enteredPassword, string storedHash, byte[] salt)
    {

        string enteredPasswordHash = HashPassword(enteredPassword, salt);
        return enteredPasswordHash == storedHash;
    }

    public static byte[] GenerateSalt()
    {
        byte[] salt = new byte[32];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
    public static string generateJwtToken(User user,String secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserID.ToString()) }),
            Expires = DateTime.UtcNow.AddYears(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

