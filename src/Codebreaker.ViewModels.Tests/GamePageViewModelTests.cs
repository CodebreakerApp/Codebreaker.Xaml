

using Codebreaker.ViewModels.Contracts.Services;

namespace CodeBreaker.ViewModels.Tests;

public class GamePageViewModelTests
{
    private readonly Mock<IGamesClient> _gamesClientMock;
    private readonly Mock<IInfoBarService> _infoBarServiceMock;

    public GamePageViewModelTests()
    {
        (Guid GameType, int NumberCodes, int MaxMoves, IDictionary<string, string[]> FieldValues) returnValue = 
            (Guid.NewGuid(), 4, 12, new Dictionary<string, string[]>()
            {
                { "colors", ["Black", "White", "Red", "Green", "Blue", "Yellow"] }
            });

        _gamesClientMock = new();
        _gamesClientMock.Setup(client => client.StartGameAsync(GameType.Game6x4, "Test", CancellationToken.None)).ReturnsAsync(returnValue);

        _infoBarServiceMock = new();
    }
    
    [Fact]
    public async Task TestGameModeStartedAfterStart()
    {
        var viewModel = new GamePageViewModel(_gamesClientMock.Object, _infoBarServiceMock.Object);
        viewModel.Name = "Test";
        await viewModel.StartGameCommand.ExecuteAsync(null);

        Assert.Equal(GameMode.Started, _viewModel.GameStatus);
    }

    [Fact]
    public async Task TestInProgressNotificationAfterStart()
    {
        var viewModel = new GamePageViewModel(_gamesClientMock.Object, _infoBarServiceMock.Object);
        List<bool> expected = new() { true, false };
        viewModel.Name = "Test";
        List<bool> inProgressValues = new();

        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName is "InProgress")
            {
                inProgressValues.Add(_viewModel.InProgress);
            }
        };
        
        await viewModel.StartGameCommand.ExecuteAsync(null);

        Assert.Equal(expected, inProgressValues);
    }
}
