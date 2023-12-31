

using Codebreaker.ViewModels.Contracts.Services;

namespace CodeBreaker.ViewModels.Tests;

public class GamePageViewModelTests
{
    private readonly GamePageViewModel _viewModel;

    public GamePageViewModelTests()
    {
        (Guid GameType, int NumberCodes, int MaxMoves, IDictionary<string, string[]> FieldValues) returnValue = 
            (Guid.NewGuid(), 4, 12, new Dictionary<string, string[]>()
            {
                { "colors", new string[] { "Black", "White", "Red", "Green", "Blue", "Yellow" } }
            });

        Mock<IGamesClient> gameClient = new();
        gameClient.Setup(
            client => client.StartGameAsync(GameType.Game6x4, "Test", CancellationToken.None)).ReturnsAsync(returnValue);

        Mock<IOptions<GamePageViewModelOptions>> options = new();
        options.Setup(o => o.Value).Returns(new GamePageViewModelOptions());

        Mock<IDialogService> dialogService = new();
        Mock<IInfoBarService> infoBarService = new();

        _viewModel = new GamePageViewModel(gameClient.Object, options.Object, dialogService.Object, infoBarService.Object);
    }
    
    [Fact]
    public async Task TestGameModeStartedAfterStart()
    {
        _viewModel.Name = "Test";
        await _viewModel.StartGameCommand.ExecuteAsync(null);

        Assert.Equal(GameMode.Started, _viewModel.GameStatus);
    }

    [Fact]
    public async Task TestInProgressNotificationAfterStart()
    {
        List<bool> expected = new() { true, false };
        _viewModel.Name = "Test";
        List<bool> inProgressValues = new();

        _viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName is "InProgress")
            {
                inProgressValues.Add(_viewModel.InProgress);
            }
        };
        
        await _viewModel.StartGameCommand.ExecuteAsync(null);

        Assert.Equal(expected, inProgressValues);
    }
}
