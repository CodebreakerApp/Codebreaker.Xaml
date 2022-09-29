﻿using System.Collections.ObjectModel;
using CodeBreaker.Shared.Models.Data;

namespace CodeBreaker.ViewModels.Components;

public class GameViewModel
{
    private readonly Game _game;

    public GameViewModel(Game game)
    {
        _game = game;
    }

    public Guid GameId => _game.GameId;

    public string Name => _game.Username;

    public string GameType => _game.Type.Name;

    public IReadOnlyList<string> Code => _game.Code;

    public IReadOnlyList<string> ColorList => _game.Type.Fields;

    public int Holes => _game.Type.Holes;

    public int MaxMoves => _game.Type.MaxMoves;

    public DateTime StartTime => _game.Start;

    public ObservableCollection<MoveViewModel> Moves { get; init; } = new ObservableCollection<MoveViewModel>();
}
