namespace BaseDotnet.Core.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("mst_users", Schema = "master")]
public class User
{
    [Key]
    [Column("user_id")]
    public int UserID { get; set; }

    [Column("username")]
    public string UserName { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [Column("display_name")]
    public string DisplayName { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("last_login")]
    public DateTime? LastLogin { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("salt")]
    public string? Salt { get; set; }

    [Column("role_id")]
    public int RoleID { get; set; }

    [NotMapped]
    public string? RoleName { get; set; }
}