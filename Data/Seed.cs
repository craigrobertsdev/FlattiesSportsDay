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
            // context.AddRange(houses);
            // await context.SaveChangesAsync();
            //
            // var savedHouses = context.Houses.ToList();
            //
            var rooms = GenerateRooms(houses);
            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync();

            var events = GenerateEvents(houses, rooms);

            context.Events.AddRange(events);
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
                Houses = houses,
                EventOffset = 7
            },
            new Room
            {
                RoomNumber = 6,
                Houses = houses,
                EventOffset = 0
            },
            new Room
            {
                RoomNumber = 7,
                Houses = houses,
                EventOffset = 5
            },
            new Room
            {
                RoomNumber = 8,
                Houses = houses,
                EventOffset = 6
            },
            new Room
            {
                RoomNumber = 12,
                Houses = houses,
                EventOffset = 1,
            },
            new Room
            {
                RoomNumber = 13,
                Houses = houses,
                EventOffset = 2,
            },
            new Room
            {
                RoomNumber = 14,
                Houses = houses,
                EventOffset = 3,
            },
            new Room
            {
                RoomNumber = 15,
                Houses = houses,
                EventOffset = 4,
            }
        ];
    }

    private List<Event> GenerateEvents(List<House> houses, List<Room> rooms)
    {
        var events = new List<Event>();
        foreach (var room in rooms)
        {
            foreach (var house in houses)
            {
                foreach (var eventName in AppConstants.ClassEvents)
                {
                    events.Add(new(eventName, room.Id, house.Id));
                }
            }
        }

        return events;
    }
}