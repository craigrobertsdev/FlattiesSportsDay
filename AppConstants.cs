using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SportsDayScoring;

public static class AppConstants
{
    static AppConstants()
    {
    }

    public static readonly int[] RoomNumbers = [5, 6, 7, 8, 12, 13, 14, 15];

    public static readonly string[] ClassEvents =
        ["Long Jump", "Gaga Ball", "Bombardment", "Marathon", "Hurdles", "Nerf Javelin", "Spoke Relay", "Shot Put"];

    public static readonly string[] SchoolEvents =
        ["Sprints", "Baton Relay", "Ribbon Relay", "Tug of War", "Team Chants"];

    public static readonly string[] HouseTextColours =
        ["text-red-400", "text-blue-400", "text-green-400", "text-yellow-400"];

    public static readonly string[] HouseBackgroundColours =
        ["bg-red-400", "bg-blue-400", "bg-green-400", "bg-yellow-400"];

    // public static readonly string[] HouseNames = ["Sturt", "Wickham", "Elliott", "Leslie"];
    
    public static HouseName[] HouseNames = (HouseName[])Enum.GetValues(typeof(HouseName));

    public static readonly int[] AthleticScores = [10, 20, 30, 40];
    public static readonly int[] SpiritScores = [0, 10, 20, 30, 40];
}

public enum HouseName
{
    Sturt,
    Wickham,
    Elliott,
    Leslie,
}