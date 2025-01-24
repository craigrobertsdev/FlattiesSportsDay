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
}
