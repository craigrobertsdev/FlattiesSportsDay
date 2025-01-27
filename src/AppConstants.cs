using SportsDayScoring.Components;

namespace SportsDayScoring;

public static class AppConstants
{
    public static readonly int[] RoomNumbers = [5, 6, 7, 8, 12, 13, 14, 15];

    public static readonly string[] HouseEvents =
        ["Long Jump", "Gaga Ball", "Bombardment", "Marathon", "Hurdles", "Nerf Javelin", "Spoke Relay", "Shot Put"];

    public static readonly string[] SchoolEvents =
        ["Sprints", "Baton Relay", "Ribbon Relay", "Tug of War", "Team Chants"];

    public static readonly HouseName[] HouseNames = (HouseName[])Enum.GetValues(typeof(HouseName));

    public static readonly  Dictionary<HouseName, HouseStyle> HouseStyles = new Dictionary<HouseName, HouseStyle>()
    {
        { HouseName.Sturt, new HouseStyle("text-red-500", "bg-red-500") },
        { HouseName.Wickham, new HouseStyle("text-blue-400", "bg-blue-400") },
        { HouseName.Elliott, new HouseStyle("text-green-400", "bg-green-400") },
        { HouseName.Leslie, new HouseStyle("text-yellow-400", "bg-yellow-400") }
    };

    public record HouseStyle(string TextColour, string BackgroundColour);

    public static readonly int[] AthleticScores = [10, 20, 30, 40];
    public static readonly int[] SpiritScores = [0, 10, 20, 30, 40];
}