using System.ComponentModel.DataAnnotations;

namespace BaseDotnet.Models;

public class ProductCategoryRequest
{
    [Required]
    public string ProductCategoryName { get; set; }

    public string ProductCategoryDescription { get; set; }
}