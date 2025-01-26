namespace SportsDayScoring.Models;

public class ScoreCard
{
    public HouseName HouseName { get; set; }
    public int AthleticPoints { get; set; }
    public int SpiritPoints { get; set; }

    public ScoreCard(HouseName houseName)
    {
        HouseName = houseName;
    }
}
