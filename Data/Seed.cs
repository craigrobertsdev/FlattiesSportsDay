using SportsDayScoring.Models;

namespace SportsDayScoring.Data;

public static class Seed {
    public static List<string> HouseNames { get; set; } = ["Sturt", "Wickham", "Elliott", "Leslie"];
    public static List<string> ClassEvents { get; set; } = ["Long Jump", "Gaga Ball", "Bombardment", "Marathon", "Hurdles", "Nerf Javelin", "Spoke Relay", "Shot Put"];

    public static List<int> RoomNumbers { get; set; } = [5, 6, 7, 8, 12, 13, 14, 15];
    public static List<Class> Rooms = [
        new Class {
            RoomNumber = 5,
            Houses = GetHouses(7)
            },
        new Class {
            RoomNumber = 6,
            Houses = GetHouses(0),
            },
        new Class {
            RoomNumber = 7,
            Houses = GetHouses(5)
            },
        new Class {
            RoomNumber = 8,
            Houses = GetHouses(6),
        },
        new Class {
            RoomNumber = 12,
            Houses = GetHouses(1),
        },
        new Class {
            RoomNumber = 13,
            Houses = GetHouses(2),
        },
        new Class {
            RoomNumber = 14,
            Houses = GetHouses(3),
        },
        new Class {
            RoomNumber = 15,
            Houses = GetHouses(4),
        }
        ];

    // Sturt - red
    // Wickham - blue
    // Elliott - green
    // Leslie - yellow

    public static List<Event> GetClassEvents(int offset) {
        List<Event> events = [];
        var idx = offset;
        while (events.Count < ClassEvents.Count) {
            if (idx == ClassEvents.Count) {
                idx = 0;
            }

            events.Add(new Event {
                Name = ClassEvents[idx]
            });

            idx++;
        }

        return events;
    }

    public static List<House> GetHouses(int offset) {
        List<House> houses = [];
        foreach (var houseName in HouseNames) {
            houses.Add(new House {
                Name = houseName,
                Events = GetClassEvents(offset)
            });
        }
        return houses;

    }
}