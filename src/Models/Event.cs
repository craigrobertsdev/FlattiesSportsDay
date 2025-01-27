namespace SportsDayScoring.Models;

public class Event
{
    public Guid Id { get; set; }
    public int EventNumber { get; set; }
    public string Name { get; set; }
    public List<ScoreCard> ScoreCards { get; set; } = [];
    public bool IsSaved { get; set; }
    public Room Room { get; set; }

    public Event(string name, Room room, int eventNumber)
    {
        Name = name;
        EventNumber = eventNumber;
        Room = room;

        foreach (var houseName in AppConstants.HouseNames)
        {
            ScoreCards.Add(new ScoreCard(houseName));
        }
    }

    private Event()
    {
    }
}