using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsDayScoring.Models;

public class Event
{
    public Guid RoomId { get; set; }
    public Guid HouseId { get; set; }
    public string Name { get; set; }
    public int AthleticScore { get; set; }
    public int SpiritScore { get; set; }
    public bool IsSaved { get; set; }

    public Event(string name, Guid roomId, Guid houseId)
    {
        RoomId = roomId;
        HouseId = houseId;
        Name = name;
    }
}