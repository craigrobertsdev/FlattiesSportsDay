namespace SportsDayScoring.Models;

public class Event {
    public string Name { get; set; } = string.Empty;
    public int AthleticScore { get; set; }
    public int SpiritScore { get; set; }
    public bool IsSaved { get; set; }
}
