using Microsoft.EntityFrameworkCore;
using SportsDayScoring.Models;

namespace SportsDayScoring.Data;

public class DataService
{
    private readonly ApplicationDbContext _context;

    public DataService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateWholeSchoolEventScores(List<ScoreCard> scoreCards)
    {
        var houses = await _context.Houses
            .ToListAsync();

        foreach (var scoreCard in scoreCards)
        {
            houses.First(h => h.Name == scoreCard.HouseName).SchoolEventAthleticScore += scoreCard.AthleticPoints;
            houses.First(h => h.Name == scoreCard.HouseName).SchoolEventSpiritScore += scoreCard.SpiritPoints;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<ScoreCard>> GetHousesWithAggregatedScores()
    {
        var houses = await _context.Houses
            .ToListAsync();

        var events = await _context.HouseEvents
            .Include(e => e.ScoreCards)
            .ToListAsync();

        var houseSpirits = await _context.HouseSpirits.ToListAsync();

        var scoreCards = new List<ScoreCard>();

        foreach (var house in houses)
        {
            var scoreCard = new ScoreCard(house.Name);

            scoreCard.AthleticPoints += house.SchoolEventAthleticScore;
            scoreCard.SpiritPoints += house.SchoolEventSpiritScore;

            foreach (var houseEvent in events)
            {
                scoreCard.AthleticPoints += houseEvent.ScoreCards.Get(house.Name).AthleticPoints;
                scoreCard.SpiritPoints += houseEvent.ScoreCards.Get(house.Name).SpiritPoints;
            }
            
            scoreCard.SpiritPoints += houseSpirits.First(hs => hs.Name == house.Name).SpiritScore;

            scoreCards.Add(scoreCard);
        }

        return scoreCards;
    }

    public async Task<Room> GetRoom(int roomNumber)
    {
        var room = await _context.Rooms
            .Where(r => r.RoomNumber == roomNumber)
            .Include(r => r.HouseEvents)
            .FirstAsync();

        room.HouseEvents.Sort((a, b) => a.EventNumber < b.EventNumber ? -1 : 1);

        var events = new List<HouseEvent>();
        var idx = room.EventOrderOffset;

        while (events.Count < room.HouseEvents.Count)
        {
            if (idx >= room.HouseEvents.Count)
            {
                idx = 0;
            }

            events.Add(room.HouseEvents[idx]);
            idx++;
        }

        room.HouseEvents = events;

        return room;
    }

    public async Task UpdateHouseEvent(HouseEvent houseEvent)
    {
        houseEvent.IsSaved = true;
        
        _context.HouseEvents.Update(houseEvent);
        await _context.SaveChangesAsync();
    }

    public async Task<List<HouseSpirit>> GetHouseSpirit()
    {
        return await _context.HouseSpirits.ToListAsync();
    }

    public async Task UpdateHouseSpirit(List<HouseSpirit> houses)
    {
        _context.HouseSpirits.UpdateRange(houses);
        await _context.SaveChangesAsync();
    }
}