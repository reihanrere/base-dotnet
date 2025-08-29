namespace BaseDotnet.Models;

using System.ComponentModel.DataAnnotations;

public class OptionResponse
{
    [Required]
    public string label { get; set; }

    [Required]
    public string value { get; set; }

    public string desc { get; set; }
}