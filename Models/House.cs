namespace SportsDayScoring.Models;

public class House {
    public string Name { get; set; } = string.Empty;
    public List<Event> Events { get; set; } = [];
}
