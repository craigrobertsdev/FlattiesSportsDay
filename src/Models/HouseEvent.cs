namespace SportsDayScoring.Models;

public class HouseEvent
{
    public Guid Id { get; init; }
    public int EventNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<ScoreCard> ScoreCards { get; init; } = [];
    public bool IsSaved { get; set; }
    public Room Room { get; init; } = null!;

    public HouseEvent(string name, Room room, int eventNumber)
    {
        Name = name;
        EventNumber = eventNumber;
        Room = room;

        foreach (var houseName in AppConstants.HouseNames)
        {
            ScoreCards.Add(new ScoreCard(houseName));
        }
    }

    private HouseEvent()
    {
    }
}