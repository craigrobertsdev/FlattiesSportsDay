using SportsDayScoring.Data;
using SportsDayScoring.Models;

namespace SportsDayScoring.Tests;

public static class TestHelpers
{
    public static async Task<Dictionary<string, Dictionary<HouseName, int>>> CompleteAllEvents(
        ApplicationDbContext dbContext)
    {
        Dictionary<string, Dictionary<HouseName, int>> results = new();

        results.Add("athletics", new Dictionary<HouseName, int>()
        {
            { HouseName.Sturt, 0 },
            { HouseName.Wickham, 0 },
            { HouseName.Elliott, 0 },
            { HouseName.Leslie, 0 }
        });
        results.Add("spirit", new Dictionary<HouseName, int>()
        {
            { HouseName.Sturt, 0 },
            { HouseName.Wickham, 0 },
            { HouseName.Elliott, 0 },
            { HouseName.Leslie, 0 }
        });

        var events = dbContext.HouseEvents.ToList();
        foreach (var ev in events)
        {
            foreach (var house in ev.ScoreCards)
            {
                house.AthleticPoints += house.HouseName switch
                {
                    HouseName.Sturt => 10,
                    HouseName.Wickham => 20,
                    HouseName.Elliott => 30,
                    HouseName.Leslie => 40,
                    _ => throw new Exception("HouseName does not exist")
                };

                house.SpiritPoints += house.HouseName switch
                {
                    HouseName.Leslie => 10,
                    HouseName.Elliott => 20,
                    HouseName.Wickham => 30,
                    HouseName.Sturt => 40,
                    _ => throw new Exception("HouseName does not exist")
                };
            }

            UpdateDictionary(results, ev);
            await dbContext.SaveChangesAsync();
        }

        return results;
    }

    private static void UpdateDictionary(Dictionary<string, Dictionary<HouseName, int>> results, HouseEvent ev)
    {
        foreach (var house in ev.ScoreCards)
        {
            results["athletics"][house.HouseName] += house.AthleticPoints;
        }

        foreach (var house in ev.ScoreCards)
        {
            results["spirit"][house.HouseName] += house.SpiritPoints;
        }
    }
}