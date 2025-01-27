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
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var houses = GenerateHouses();
            var rooms = GenerateRooms(houses);
            
            context.Rooms.AddRange(rooms);
            context.Houses.AddRange(houses);
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

    private static List<House> GenerateHouses()
    {
        var houses = new List<House>();
        for (int i = 0; i < AppConstants.HouseNames.Length; i++)
        {
            houses.Add(new House()
            {
                Name = AppConstants.HouseNames[i],
            });
        }

        return houses;
    }

    private List<Room> GenerateRooms(List<House> houses)
    {
        return
        [
            new Room
            {
                RoomNumber = 5,
                EventOrderOffset = 7
            },
            new Room
            {
                RoomNumber = 6,
                EventOrderOffset = 0
            },
            new Room
            {
                RoomNumber = 7,
                EventOrderOffset = 5
            },
            new Room
            {
                RoomNumber = 8,
                EventOrderOffset = 6
            },
            new Room
            {
                RoomNumber = 12,
                EventOrderOffset = 1,
            },
            new Room
            {
                RoomNumber = 13,
                EventOrderOffset = 2,
            },
            new Room
            {
                RoomNumber = 14,
                EventOrderOffset = 3,
            },
            new Room
            {
                RoomNumber = 15,
                EventOrderOffset = 4,
            }
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