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

    public async Task UpdateWholeSchoolEventScores(string eventName, List<ScoreCard> scoreCards)
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

        var events = await _context.Events
            .Include(e => e.ScoreCards)
            .ToListAsync();

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

            scoreCards.Add(scoreCard);
        }

        return scoreCards;
    }

    public async Task<Room> GetRoom(int roomNumber)
    {
        var room = await _context.Rooms
            .Where(r => r.RoomNumber == roomNumber)
            .Include(r => r.Events)
            .FirstAsync();

        room.Events.Sort((a, b) => a.EventNumber < b.EventNumber ? -1 : 1);

        var events = new List<Event>();
        var idx = room.EventOrderOffset;

        while (events.Count < room.Events.Count)
        {
            if (idx >= room.Events.Count)
            {
                idx = 0;
            }

            events.Add(room.Events[idx]);
            idx++;
        }

        room.Events = events;

        return room;
    }

    public async Task UpdateEvent(Event houseEvent)
    {
        _context.Events.Update(houseEvent);
        await _context.SaveChangesAsync();
    }
}