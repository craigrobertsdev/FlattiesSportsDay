namespace SportsDayScoring.Models;

public class HouseScoreCard(HouseName houseName)
{
    public readonly HouseName HouseName = houseName;
    public int AthleticPoints { get; set; }
    public int SpiritPoints { get; set; }
}
