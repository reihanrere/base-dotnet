namespace BaseDotnet.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("mst_office", Schema = "master")]
public class Office
{
    [Key]
    [Column("office_id")]
    [JsonIgnore]
    public int OfficeId { get; set; }

    [Column("office_name")]
    public string OfficeName { get; set; }

    [Column("office_address")]
    public string OfficeAddress { get; set; }

    [Column("office_longitude")]
    public string OfficeLongitude { get; set; }

    [Column("office_latitude")]
    public string OfficeLatitude { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("update_at")]
    public DateTime? UpdateAt { get; set; }
}