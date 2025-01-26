namespace SportsDayScoring.Models;

public class Room {
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public List<House> Houses { get; set; } = [];
    public int EventOffset { get; set; }
}
