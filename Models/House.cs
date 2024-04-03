using System.ComponentModel.DataAnnotations;

namespace SportsDayScoring.Models;

public class House {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Event> Events { get; set; } = [];
}
