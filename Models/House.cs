using System.ComponentModel.DataAnnotations;

namespace SportsDayScoring.Models;

public class House {
    public Guid Id { get; set; }
    public HouseName Name { get; set; }
    public List<Event> Events { get; set; } = [];
    public int SchoolEventAthleticScore { get; set; }
    public int SchoolEventSpiritScore { get; set; }
}
