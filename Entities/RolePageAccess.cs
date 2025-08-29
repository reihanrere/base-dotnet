namespace BaseDotnet.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("mst_role_page_access", Schema = "master")]
public class RolePageAccess
 {
    [Column("page_code")]
    public string PageCode { get; set; }

    [Column("action")]
    public string Action { get; set; }

    [Column("role_id")]
    public int RoleID { get; set; }
}