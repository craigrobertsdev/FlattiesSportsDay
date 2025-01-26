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

    public async Task<List<House>> GetHousesAsync(int roomNumber)
    {
        var room = await _context.Rooms
            .Where(r => r.RoomNumber == roomNumber)
            .Include(r => r.Houses)
            .ThenInclude(h => h.Events)
            .FirstOrDefaultAsync();

        if (room is null)
        {
            throw new Exception("Room not found");
        }

        return room.Houses;
    }

    public async Task UpdateRoomScores(Room newRoom)
    {
        var room = await _context.Rooms
            .Where(r => r.RoomNumber == newRoom.RoomNumber)
            .Include(r => r.Houses)
            .ThenInclude(h => h.Events)
            .FirstOrDefaultAsync();
        
        if (room is null)
        {
            throw new Exception("Room not found");
        }

        foreach (var house in newRoom.Houses)
        {
            room.Houses = newRoom.Houses;
        }
        
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task<List<House>> GetHousesWithSchoolEvents()
    {
        return await _context.Houses
            .Include(h => h.Events)
            .ToListAsync();
    }

    public async Task UpdateWholeSchoolEventScores(string eventName, List<HouseScoreCard> scoresCards)
    {
        var houses = await _context.Houses
            .ToListAsync();

        foreach (var scoreCard in scoresCards)
        {
            houses.First(h => h.Name == scoreCard.HouseName).SchoolEventAthleticScore += scoreCard.AthleticPoints;
            houses.First(h => h.Name == scoreCard.HouseName).SchoolEventSpiritScore += scoreCard.SpiritPoints;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<List<HouseScoreCard>> GetHousesWithAggregatedScores()
    {
        var houses = await _context.Houses
            .Include(h => h.Events)
            .ToListAsync();
        
        var scoreCards = new List<HouseScoreCard>();
        foreach (var house in houses)
        {
            var scoreCard = new HouseScoreCard(house.Name);

            foreach (var houseEvent in house.Events)
            {
                scoreCard.AthleticPoints += houseEvent.AthleticScore;
                scoreCard.SpiritPoints += houseEvent.SpiritScore;
            }

            scoreCard.AthleticPoints += house.SchoolEventAthleticScore;
            scoreCard.SpiritPoints += house.SchoolEventSpiritScore;
            
            scoreCards.Add(scoreCard);
        }
        
        return scoreCards;
    }
}
