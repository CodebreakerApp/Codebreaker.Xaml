﻿using Codebreaker.ViewModels.Contracts.Services;
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

    [ObservableProperty]
    private GameType _selectedGameType = GameType.Game6x4;

    public IEnumerable<GameType> GameTypes { get; } = [
        GameType.Game6x4,
        GameType.Game6x4Mini,
        GameType.Game8x5,
        GameType.Game5x5x4
    ];

    private bool CanStartGame() => Username is not null && Username.Length > 2;

    [RelayCommand(CanExecute = nameof(CanStartGame))]
    private async Task StartGameAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;
        
        try
        {
            var response = await gamesClient.StartGameAsync(SelectedGameType, Username);
            Game = new Game(response.Id, SelectedGameType, Username, DateTime.Now, response.NumberCodes, response.MaxMoves, response.FieldValues);
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

        WeakReferenceMessenger.Default.Send(new GameStartedMessage(Game));

        // Initialize SelectedFields
        SelectedFields = Enumerable.Range(0, Game.NumberCodes)
            .Select(i =>
            {
                var field = new Field();
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

        if (SelectedFields.Any(field => field.Color is null))
            throw new InvalidOperationException("All colors need to be selected before making a move");

        WeakReferenceMessenger.Default.Send(new MakeMoveMessage(new(SelectedFields)));

        var serializedFields = SelectedFields.Select(field => field.Serialize()).ToArray();
        IsLoading = true;
        try
        {
            var response = await gamesClient.SetMoveAsync(Game.Id, Game.PlayerName, Game.GameType, Game.Moves.Count + 1, serializedFields);

            // It is necessary to copy the fields to avoid every move having the same reference to the same fields
            var copiedFields = SelectedFields.Select(f => new Field(f.Color, f.Shape)).ToArray();
            var newMove = new Move(copiedFields, response.Results);
            Game.Moves.Add(newMove);
            WeakReferenceMessenger.Default.Send(new MakeMoveMessage(newMove));

            if (response.Ended)
            {
                Game.EndTime = DateTime.Now;
                Game.IsVictory = response.IsVictory;
                WeakReferenceMessenger.Default.Send(new GameEndedMessage(Game));
            }
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
    }

    private bool CanCancelGame() => Game is not null && !Game.IsFinished;

    [RelayCommand(CanExecute = nameof(CanCancelGame))]
    private async Task CancelGameAsync(CancellationToken cancellationToken)
    {
        if (Game is null)
            throw new InvalidOperationException("Cannot cancel a not-started game.");

        try
        {
            await gamesClient.CancelGameAsync(Game.Id, Game.PlayerName, Game.GameType, cancellationToken);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
        {
            infoBarService.New.WithMessage("Could not cancel the game").Show();
            return;
        }
        catch (HttpRequestException)
        {
            infoBarService.New.WithMessage("Networking issues").Show();
            return;
        }

        Game = null;
        WeakReferenceMessenger.Default.Send(new GameCancelledMessage());
    }
}