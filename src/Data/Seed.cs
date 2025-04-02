using Microsoft.EntityFrameworkCore;
using SportsDayScoring.Models;

namespace SportsDayScoring.Data;

public class Seed(ApplicationDbContext context)
{
    public async Task SeedData()
    {
        // Change the way that IDs are generated so the DB does it. After each type of object is saved, retrieve those objects from the database and use the DB generated Ids to create the other objects.
        try
        {
            context.HouseEvents.RemoveRange(context.HouseEvents);
            context.HouseSpirits.RemoveRange(context.HouseSpirits);
            context.Rooms.RemoveRange(context.Rooms);
            context.Houses.RemoveRange(context.Houses);
            await context.SaveChangesAsync();

            var houses = GenerateHouses();
            var rooms = GenerateRooms();
            var houseSpirits = GenerateHouseSpirits();

            context.Houses.AddRange(houses);
            context.Rooms.AddRange(rooms);
            context.HouseSpirits.AddRange(houseSpirits);
            await context.SaveChangesAsync();

            var roomsWithIds = await context.Rooms.ToListAsync();
            var events = GenerateEvents(roomsWithIds);

            context.HouseEvents.AddRange(events);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private List<HouseSpirit> GenerateHouseSpirits() =>
        AppConstants.HouseNames.Select(n => new HouseSpirit(n)).ToList();

    private static List<House> GenerateHouses() =>
        AppConstants.HouseNames.Select(n => new House(n)).ToList();

    private List<Room> GenerateRooms()
    {
        return
        [
            new Room(5, 7),
            new Room(6, 0),
            new Room(7, 5),
            new Room(8, 6),
            new Room(12, 1),
            new Room(13, 2),
            new Room(14, 3),
            new Room(15, 4)
        ];
    }

    private List<HouseEvent> GenerateEvents(List<Room> rooms)
    {
        var events = new List<HouseEvent>();
        foreach (var room in rooms)
        {
            for (int i = 0; i < AppConstants.HouseEvents.Length; i++)
            {
                events.Add(new(AppConstants.HouseEvents[i], room, i));
            }
        }

        return events;
    }
}