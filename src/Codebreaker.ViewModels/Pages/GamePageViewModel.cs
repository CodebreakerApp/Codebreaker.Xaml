using Codebreaker.ViewModels.Contracts.Services;
using Codebreaker.ViewModels.Messages;
using System.ComponentModel;
using System.Net;

namespace Codebreaker.ViewModels;

public partial class GamePageViewModel(IGamesClient gamesClient, IInfoBarService infoBarService) : ObservableRecipient
{
    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private Game? _game;

    [ObservableProperty]
    private Field[] _selectedFields = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartGameCommand))]
    private string _username = string.Empty;

    private bool CanStartGame() => Username is not null && Username.Length > 2;

    [RelayCommand(CanExecute = nameof(CanStartGame))]
    private async Task StartGameAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;
        var usedGameMode = GameType.Game6x4;
        (Guid id, int numberCode, int maxMoves, IDictionary<string, string[]> fieldValues) response;
        
        try
        {
            response = await gamesClient.StartGameAsync(usedGameMode, Username);
        }
        catch (InvalidOperationException)
        {
            infoBarService.New.WithMessage("Invalid operation").Show();
            return;
        }
        catch(HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            infoBarService.New.WithMessage(ex.Message).Show();
            return;
        }
        catch (HttpRequestException)
        {
            infoBarService.New.WithMessage("Networking issues").Show();
            return;
        }
        finally
        {
            IsLoading = false;
        }

        Game = new Game(response.id, usedGameMode, Username, DateTime.Now, response.numberCode, response.maxMoves, response.fieldValues);

        WeakReferenceMessenger.Default.Send(new GameStartedMessage(Game));

        // Initialize SelectedFields
        SelectedFields = Enumerable.Range(0, Game.NumberCodes)
            .Select(i =>
            {
                var field = new Field(Game.FieldValues["colors"]);   // TODO: Hardcoding "colors" is not suitable for all game types
                field.PropertyChanged += (object? sender, PropertyChangedEventArgs args) => MakeMoveCommand.NotifyCanExecuteChanged();
                return field;
            })
            .ToArray();
    }

    private bool CanMakeMove() => SelectedFields.All(field => field is not null);

    [RelayCommand(CanExecute = nameof(CanMakeMove))]
    private async Task MakeMoveAsync(CancellationToken cancellationToken)
    {
        if (Game is null)
            throw new InvalidOperationException("A game needs to be started before making a move");

        var selectedColors = SelectedFields
            .Select(x => x.Color)
            .ToArray();

        if (selectedColors.Any(color => color is null))
            throw new InvalidOperationException("All colors need to be selected before making a move");

        WeakReferenceMessenger.Default.Send(new MakeMoveMessage(new(selectedColors!)));

        IsLoading = true;
        (string[] keyPegs, bool hasEnded, bool isVictory) response;
        try
        {
            response = await gamesClient.SetMoveAsync(Game.Id, Game.PlayerName, GameType.Game6x4, Game.Moves.Count + 1, selectedColors!);
        }
        catch (InvalidOperationException)
        {
            infoBarService.New.WithMessage("Invalid operation").Show();
            return;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            infoBarService.New.WithMessage(ex.Message).Show();
            return;
        }
        catch (HttpRequestException)
        {
            infoBarService.New.WithMessage("Networking issues").Show();
            return;
        }
        finally
        {
            IsLoading = false;
        }

        var newMove = new Move(selectedColors!, response.keyPegs);
        Game.Moves.Add(newMove);
        WeakReferenceMessenger.Default.Send(new MakeMoveMessage(newMove));

        if (response.hasEnded)
        {
            Game.EndTime = DateTime.Now;
            Game.IsVictory = response.isVictory;
            WeakReferenceMessenger.Default.Send(new GameEndedMessage(Game));
        }
    }
}