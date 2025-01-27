namespace SportsDayScoring.Models;

public class HouseSpirit
{
    public Guid Id { get; set; }
    public HouseName Name { get; set; }
    public int SpiritScore { get; set; }

    public HouseSpirit(HouseName name)
    {
        Name = name;
    }
}