using System.ComponentModel.DataAnnotations;

namespace SportsDayScoring.Models;

public class Event {
    public int Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public int? AthleticScore { get; set; } = null;
    public int SpiritScore { get; set; }
}
