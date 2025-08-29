namespace BaseDotnet.Core.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("mst_role", Schema = "master")]
public class Role
 {
    [Key]
    [Column("role_id")]
    public int RoleID { get; set; }

    [Column("role_name")]
    public string RoleName { get; set; }

    [NotMapped]
    public List<RolePageAccess>? AccessList { get; set; }

    [NotMapped]
    public List<RolePage>? PageList { get; set; }
}