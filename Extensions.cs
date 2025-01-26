using SportsDayScoring.Models;

namespace SportsDayScoring;

public static class Extensions
{
    public static ScoreCard Get(this List<ScoreCard> scoreCards, HouseName houseName) => 
        scoreCards.First(sc => sc.HouseName == houseName);
}