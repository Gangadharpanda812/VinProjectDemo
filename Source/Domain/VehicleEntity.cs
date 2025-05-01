using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml;

namespace Domain;

[Table("vehicles")]
public class Vehicle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(17)]
    [Column("vin")]
    public required string Vin { get; set; }

    [MaxLength(50)]
    [Column("make")]
    public required string Make { get; set; }

    [MaxLength(50)]
    [Column("model")]
    public required string Model { get; set; }

    [Column("year")]
    public int? Year { get; set; }

    [MaxLength(2)]
    [Column("title_state")]
    public required string TitleState { get; set; }

    [MaxLength(20)]
    [Column("registration_status")]
    public required string RegistrationStatus { get; set; }

    [MaxLength(10)]
    [Column("emissions_status")]
    public required string EmissionsStatus { get; set; }

    [MaxLength(50)]
    [Column("recall_status")]
    public required string RecallStatus { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
 
 