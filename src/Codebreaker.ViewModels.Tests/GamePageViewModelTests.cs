

using Codebreaker.ViewModels.Contracts.Services;

namespace CodeBreaker.ViewModels.Tests;

public class GamePageViewModelTests
{
    private static readonly string[] s_code = ["Green", "Blue", "Yellow", "Orange"];
    private static readonly (string[] GuessPegs, string[] KeyPegs, bool HasEnded, bool IsVictory)[] s_moves = [
        (["Red", "Red", "Red", "Red"], [], false, false),
        (["Green", "Green", "Green", "Green"], ["Black"], false, false),
        (["Blue", "Green", "Green", "Green"], ["White", "White"], false, false),
        (["Green", "Blue", "Blue", "Blue"], ["Black", "Black"], false, false),
        (["Green", "Blue", "Yellow", "Yellow"], ["Black", "Black", "Black"], false, false),
        (["Green", "Blue", "Yellow", "Purple"], ["Black", "Black", "Black"], false, false),
        (["Green", "Blue", "Yellow", "Orange"], ["Black", "Black", "Black", "Black"], true, true),
    ];

    private readonly Mock<IGamesClient> _gamesClientMock;
    private readonly Mock<IInfoBarService> _infoBarServiceMock;

    public GamePageViewModelTests()
    {
        (Guid Id, int NumberCodes, int MaxMoves, IDictionary<string, string[]> FieldValues) gameReturnValue = 
            (Guid.NewGuid(), 4, 12, new Dictionary<string, string[]>()
            {
                { "colors", ["Black", "White", "Red", "Green", "Blue", "Yellow"] }
            });

        _gamesClientMock = new();
        // Setup the StartGameAsync method of the GamesClient.
        _gamesClientMock.Setup(client => client.StartGameAsync(GameType.Game6x4, "Test", CancellationToken.None)).ReturnsAsync(gameReturnValue);

        // Setup the SetMoveAsync method of the GamesClient.
        for (int i = 0; i < s_moves.Length; i++)
        {
            var move = s_moves[i];
            _gamesClientMock.Setup(client => client.SetMoveAsync(gameReturnValue.Id, "Test", GameType.Game6x4, i + 1, move.GuessPegs, CancellationToken.None))
                .ReturnsAsync((move.KeyPegs, move.HasEnded, move.IsVictory));
        }

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

    [Fact]
    public async Task TestMoves()
    {
        // Start game
        var viewModel = new GamePageViewModel(_gamesClientMock.Object, _infoBarServiceMock.Object);
        viewModel.Username = "Test";

        await viewModel.StartGameCommand.ExecuteAsync(null);

        // Play game
        foreach ((string[] guessPegs, _, _, _) in s_moves)
        {
            for (int i = 0; i < guessPegs.Length; i++)
                viewModel.SelectedFields[i].Color = guessPegs[i];

            await viewModel.MakeMoveCommand.ExecuteAsync(null);
        }

        Assert.Equal(s_moves.Length, viewModel.Game?.Moves.Count);
        Assert.NotNull(viewModel.Game?.EndTime);
        Assert.True(viewModel.Game?.IsVictory);
    }
}
