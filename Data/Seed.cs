using SportsDayScoring.Models;

namespace SportsDayScoring.Data;

public static class Seed {
    public static List<string> HouseNames { get; set; } = ["Sturt", "Wickham", "Elliott", "Leslie"];
    public static List<string> ClassEvents { get; set; } = ["Long Jump", "Gaga Ball", "Bombardment", "Marathon", "Hurdles", "Nerf Javelin", "Spoke Relay", "Shot Put"];
    public static List<string> SchoolEvents { get; set; } = ["Sprints", "House 100m Baton Relay", "Whole School Ribbon Relay", "Tug of War", "Team Chants"];

    public static List<int> RoomNumbers { get; set; } = [5, 6, 7, 8, 12, 13, 14, 15];
    public static List<Class> Rooms = [
        new Class {
            RoomNumber = 5,
            Houses = GetHousesClassEvents(7)
            },
        new Class {
            RoomNumber = 6,
            Houses = GetHousesClassEvents(0),
            },
        new Class {
            RoomNumber = 7,
            Houses = GetHousesClassEvents(5)
            },
        new Class {
            RoomNumber = 8,
            Houses = GetHousesClassEvents(6),
        },
        new Class {
            RoomNumber = 12,
            Houses = GetHousesClassEvents(1),
        },
        new Class {
            RoomNumber = 13,
            Houses = GetHousesClassEvents(2),
        },
        new Class {
            RoomNumber = 14,
            Houses = GetHousesClassEvents(3),
        },
        new Class {
            RoomNumber = 15,
            Houses = GetHousesClassEvents(4),
        }
        ];

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

    public static List<Event> GetSchoolEvents() {
        List<Event> events = [];
        foreach (var eventName in SchoolEvents) {
            events.Add(new Event {
                Name = eventName
            });
        }
        return events;
    }

    public static List<House> GetHousesClassEvents(int offset) {
        List<House> houses = [];
        foreach (var houseName in HouseNames) {
            houses.Add(new House {
                Name = houseName,
                Events = GetClassEvents(offset)
            });
        }
        return houses;

    }

    public static List<House> GetHousesSchoolEvents() {
        List<House> houses = [];
        foreach (var houseName in HouseNames) {
            houses.Add(new House {
                Name = houseName,
                Events = GetSchoolEvents()
            });
        }

        return houses;
    }

    public static List<HouseSpirit> GetSpirit() {
        List<HouseSpirit> houses = [];
        foreach (var houseName in HouseNames) {
            houses.Add(new HouseSpirit {
                Name = houseName,
                SpiritScore = 0
            });
        }

        return houses;
    }
}
