namespace BaseDotnet.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("mst_role_page", Schema = "master")]
public class RolePage
 {
    [Key]
    [Column("page_code")]
    public string PageCode { get; set; }

    [Column("page")]
    public string Page { get; set; }

    [Column("category")]
    public string Category { get; set; }

    [Column("action")]
    public string Action { get; set; }

}