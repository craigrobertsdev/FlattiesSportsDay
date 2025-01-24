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
}
