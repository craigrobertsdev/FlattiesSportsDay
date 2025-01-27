namespace SportsDayScoring.Models;

public class Room
{
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public List<HouseEvent> HouseEvents { get; set; } = [];
    public int EventOrderOffset { get; set; }

    public Room(int roomNumber, int eventOrderOffset)
    {
        RoomNumber = roomNumber;
        EventOrderOffset = eventOrderOffset;
    }
}