using Microsoft.EntityFrameworkCore;
using SportsDayScoring.Data;
using SportsDayScoring.Models;

namespace SportsDayScoring.Tests;

public class DataServiceTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("Data Source=test.db")
            .Options;

        var db = new ApplicationDbContext(options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        return db;
    }

    [Fact]
    public async Task UpdateWholeSchoolEventScores_WhenGivenScoreCardsAndCurrentScoresAreZero_UpdatesHouseScores()
    {
        // Arrange
        var db = GetDbContext();
        var seed = new Seed(db);
        await seed.SeedData();
        var dataService = new DataService(db);

        var scoreCards = new List<ScoreCard>
        {
            new(HouseName.Elliott) { AthleticPoints = 40, SpiritPoints = 40 },
            new(HouseName.Leslie) { AthleticPoints = 30, SpiritPoints = 30 },
            new(HouseName.Sturt) { AthleticPoints = 20, SpiritPoints = 20 },
            new(HouseName.Wickham) { AthleticPoints = 10, SpiritPoints = 10 },
        };

        // Act
        await dataService.UpdateWholeSchoolEventScores(scoreCards);

        // Assert
        var houses = await db.Houses.ToListAsync();

        Assert.Equal(40, houses.First(h => h.Name == HouseName.Elliott).SchoolEventAthleticScore);
        Assert.Equal(40, houses.First(h => h.Name == HouseName.Elliott).SchoolEventSpiritScore);

        Assert.Equal(30, houses.First(h => h.Name == HouseName.Leslie).SchoolEventAthleticScore);
        Assert.Equal(30, houses.First(h => h.Name == HouseName.Leslie).SchoolEventSpiritScore);

        Assert.Equal(20, houses.First(h => h.Name == HouseName.Sturt).SchoolEventAthleticScore);
        Assert.Equal(20, houses.First(h => h.Name == HouseName.Sturt).SchoolEventSpiritScore);

        Assert.Equal(10, houses.First(h => h.Name == HouseName.Wickham).SchoolEventAthleticScore);
        Assert.Equal(10, houses.First(h => h.Name == HouseName.Wickham).SchoolEventSpiritScore);
    }

    [Fact]
    public async Task UpdateWholeSchoolEvents_WhenGivenScoreCardsAndCurrentScoresAreNotZero_UpdatesHouseScores()
    {
        // Arrange
        var db = GetDbContext();
        var seed = new Seed(db);
        await seed.SeedData();
        var dataService = new DataService(db);

        var scoreCards = new List<ScoreCard>
        {
            new(HouseName.Elliott) { AthleticPoints = 40, SpiritPoints = 40 },
            new(HouseName.Leslie) { AthleticPoints = 30, SpiritPoints = 30 },
            new(HouseName.Sturt) { AthleticPoints = 20, SpiritPoints = 20 },
            new(HouseName.Wickham) { AthleticPoints = 10, SpiritPoints = 10 },
        };

        // Act
        await dataService.UpdateWholeSchoolEventScores(scoreCards);
        await dataService.UpdateWholeSchoolEventScores(scoreCards);

        // Assert
        var houses = await db.Houses.ToListAsync();

        Assert.Equal(80, houses.First(h => h.Name == HouseName.Elliott).SchoolEventAthleticScore);
        Assert.Equal(80, houses.First(h => h.Name == HouseName.Elliott).SchoolEventSpiritScore);

        Assert.Equal(60, houses.First(h => h.Name == HouseName.Leslie).SchoolEventAthleticScore);
        Assert.Equal(60, houses.First(h => h.Name == HouseName.Leslie).SchoolEventSpiritScore);

        Assert.Equal(40, houses.First(h => h.Name == HouseName.Sturt).SchoolEventAthleticScore);
        Assert.Equal(40, houses.First(h => h.Name == HouseName.Sturt).SchoolEventSpiritScore);

        Assert.Equal(20, houses.First(h => h.Name == HouseName.Wickham).SchoolEventAthleticScore);
        Assert.Equal(20, houses.First(h => h.Name == HouseName.Wickham).SchoolEventSpiritScore);
    }

    [Fact]
    public async Task GetHousesWithAggregatedScores_WhenCalled_ReturnsHouseScores()
    {
        // Arrange
        var db = GetDbContext();
        var seed = new Seed(db);
        await seed.SeedData();
        var dataService = new DataService(db);

        // Act
        var scoreCards = await dataService.GetHousesWithAggregatedScores();

        // Assert
        Assert.Equal(4, scoreCards.Count);

        Assert.Equal(0, scoreCards.First(s => s.HouseName == HouseName.Elliott).AthleticPoints);
        Assert.Equal(0, scoreCards.First(s => s.HouseName == HouseName.Elliott).SpiritPoints);

        Assert.Equal(0, scoreCards.First(s => s.HouseName == HouseName.Leslie).AthleticPoints);
        Assert.Equal(0, scoreCards.First(s => s.HouseName == HouseName.Leslie).SpiritPoints);

        Assert.Equal(0, scoreCards.First(s => s.HouseName == HouseName.Sturt).AthleticPoints);
        Assert.Equal(0, scoreCards.First(s => s.HouseName == HouseName.Sturt).SpiritPoints);

        Assert.Equal(0, scoreCards.First(s => s.HouseName == HouseName.Wickham).AthleticPoints);
        Assert.Equal(0, scoreCards.First(s => s.HouseName == HouseName.Wickham).SpiritPoints);
    }

    [Fact]
    public async Task UpdateHouseEvent_WhenContainedScoreCardsHaveScores_Updates()
    {
        // Arrange
        var db = GetDbContext();
        var seed = new Seed(db);
        await seed.SeedData();
        var dataService = new DataService(db);
        var houseEvent = await db.HouseEvents.FirstAsync();

        // Act
        houseEvent.ScoreCards.ForEach(sc =>
        {
            sc.AthleticPoints = 10;
            sc.SpiritPoints = 10;
        });

        await dataService.UpdateHouseEvent(houseEvent);

        // Assert
        var updatedHouseEvent = await db.HouseEvents.FirstAsync();
        Assert.True(updatedHouseEvent.IsSaved);
        updatedHouseEvent.ScoreCards.ForEach(sc =>
        {
            Assert.Equal(10, sc.AthleticPoints);
            Assert.Equal(10, sc.SpiritPoints);
        });
    }

    [Fact]
    public async Task GetHousesWithAggregatedScores_WhenScoresAreNotZero_ReturnsHouseScores()
    {
        // Arrange
        var db = GetDbContext();
        var seed = new Seed(db);
        await seed.SeedData();
        var dataService = new DataService(db);

        var scoreCards = new List<ScoreCard>
        {
            new(HouseName.Elliott) { AthleticPoints = 40, SpiritPoints = 40 },
            new(HouseName.Leslie) { AthleticPoints = 30, SpiritPoints = 30 },
            new(HouseName.Sturt) { AthleticPoints = 20, SpiritPoints = 20 },
            new(HouseName.Wickham) { AthleticPoints = 10, SpiritPoints = 10 },
        };

        await dataService.UpdateWholeSchoolEventScores(scoreCards);

        // Act
        var updatedScoreCards = await dataService.GetHousesWithAggregatedScores();

        // Assert
        Assert.Equal(4, updatedScoreCards.Count);

        Assert.Equal(40, updatedScoreCards.Get(HouseName.Elliott).AthleticPoints);
        Assert.Equal(40, updatedScoreCards.Get(HouseName.Elliott).SpiritPoints);

        Assert.Equal(30, updatedScoreCards.Get(HouseName.Leslie).AthleticPoints);
        Assert.Equal(30, updatedScoreCards.Get(HouseName.Leslie).SpiritPoints);

        Assert.Equal(20, updatedScoreCards.Get(HouseName.Sturt).AthleticPoints);
        Assert.Equal(20, updatedScoreCards.Get(HouseName.Sturt).SpiritPoints);

        Assert.Equal(10, updatedScoreCards.Get(HouseName.Wickham).AthleticPoints);
        Assert.Equal(10, updatedScoreCards.Get(HouseName.Wickham).SpiritPoints);
    }

    [Fact]
    public async Task
        GetHousesWithAggregatedScores_WhenHouseScoresAndHouseEventScoreCardsAreNotZero_ReturnsHouseScores()
    {
        // Arrange
        var db = GetDbContext();
        var seed = new Seed(db);
        await seed.SeedData();
        var dataService = new DataService(db);

        var scoreCards = new List<ScoreCard>
        {
            new(HouseName.Elliott) { AthleticPoints = 40, SpiritPoints = 40 },
            new(HouseName.Leslie) { AthleticPoints = 30, SpiritPoints = 30 },
            new(HouseName.Sturt) { AthleticPoints = 20, SpiritPoints = 20 },
            new(HouseName.Wickham) { AthleticPoints = 10, SpiritPoints = 10 },
        };

        await dataService.UpdateWholeSchoolEventScores(scoreCards);

        var houseEvent = await db.HouseEvents.FirstAsync();
        houseEvent.ScoreCards.ForEach(sc =>
        {
            sc.AthleticPoints = 10;
            sc.SpiritPoints = 10;
        });


        // Act
        var updatedScoreCards = await dataService.GetHousesWithAggregatedScores();

        // Assert
        Assert.Equal(4, updatedScoreCards.Count);

        Assert.Equal(50, updatedScoreCards.Get(HouseName.Elliott).AthleticPoints);
        Assert.Equal(50, updatedScoreCards.Get(HouseName.Elliott).SpiritPoints);

        Assert.Equal(40, updatedScoreCards.Get(HouseName.Leslie).AthleticPoints);
        Assert.Equal(40, updatedScoreCards.Get(HouseName.Leslie).SpiritPoints);

        Assert.Equal(30, updatedScoreCards.Get(HouseName.Sturt).AthleticPoints);
        Assert.Equal(30, updatedScoreCards.Get(HouseName.Sturt).SpiritPoints);

        Assert.Equal(20, updatedScoreCards.Get(HouseName.Wickham).AthleticPoints);
        Assert.Equal(20, updatedScoreCards.Get(HouseName.Wickham).SpiritPoints);
    }

    [Fact]
    public async Task GetRoom_WhenCalled_ReturnsRoomWithEventsOrderedCorrectly()
    {
        // Arrange
        var db = GetDbContext();
        var seed = new Seed(db);
        await seed.SeedData();
        var dataService = new DataService(db);

        // Act
        var room = await dataService.GetRoom(5);

        // Assert
        Assert.Equal(5, room.RoomNumber);
        Assert.Equal(8, room.HouseEvents.Count);
        Assert.Equal(7, room.HouseEvents.First().EventNumber);
        Assert.Equal(6, room.HouseEvents.Last().EventNumber);
    }
}