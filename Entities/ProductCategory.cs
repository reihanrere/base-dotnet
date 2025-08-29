namespace BaseDotnet.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("mst_product_category", Schema = "master")]
public class ProductCategory
{
    [Key]
    [Column("product_category_id")]
    public int ProductCategoryID { get; set; }

    [Column("name")]
    public string ProductCategoryName { get; set; }

    [Column("description")]
    public string ProductCategoryDescription { get; set; }
}