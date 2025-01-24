using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsDayScoring.Models;

public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AthleticScore { get; set; }
    public int SpiritScore { get; set; }
    public bool IsSaved { get; set; }
}