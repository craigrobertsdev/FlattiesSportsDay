namespace SportsDayScoring.Models;

public class Room {
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public List<Event> Events { get; set; } = [];
    public int EventOrderOffset { get; set; }
}
