using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml;

namespace Domain;

[Table("apilogs", Schema = "public")]
public class ApiLogs
{
    [Key]
    [Column("transactionid")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long TransactionId { get; set; }

    [Column("logtype")]
    [Required]
    public string LogType { get; set; }

    [Column("start_time")]
    [Required]
    public DateTime StartTime { get; set; }

    [Column("end_time")]
    public DateTime? EndTime { get; set; }

    [Column("duration")]
    public float? Duration { get; set; }

    [Column("requestmethod")]
    [Required]
    public string RequestMethod { get; set; }

    [Column("requestpath")]
    [Required]
    public string RequestPath { get; set; }

    [Column("responsestatuscode")]
    public int? ResponseStatusCode { get; set; }
}

