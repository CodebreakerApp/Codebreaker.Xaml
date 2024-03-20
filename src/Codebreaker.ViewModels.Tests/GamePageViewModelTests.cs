

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
    public async Task TestGameStart()
    {
        var viewModel = new GamePageViewModel(_gamesClientMock.Object, _infoBarServiceMock.Object);
        viewModel.Username = "Test";
        await viewModel.StartGameCommand.ExecuteAsync(null);

        Assert.Equal(4, viewModel.SelectedFields.Length);
        Assert.All(viewModel.SelectedFields, field => Assert.NotNull(field));
        Assert.NotNull(viewModel.Game);
    }

    [Fact]
    public async Task TestIsLoadingNotificationAfterStart()
    {
        var viewModel = new GamePageViewModel(_gamesClientMock.Object, _infoBarServiceMock.Object);
        List<bool> expectedIsLoadingValues = [true, false];
        List<bool> actualInProgressValues = [];
        viewModel.Username = "Test";

        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName is nameof(GamePageViewModel.IsLoading))
                actualInProgressValues.Add(viewModel.IsLoading);
        };
        
        await viewModel.StartGameCommand.ExecuteAsync(null);

        Assert.Equal(expectedIsLoadingValues, actualInProgressValues);
    }
}
