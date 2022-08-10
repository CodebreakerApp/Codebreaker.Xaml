﻿using System.Collections.ObjectModel;
using CodeBreaker.Services;
using CodeBreaker.Shared;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CodeBreaker.ViewModels;

[ObservableObject]
public partial class LivePageViewModel
{
    private readonly LiveClient _liveClient;

    public LivePageViewModel(LiveClient liveClient)
    {
        _liveClient = liveClient;
        _liveClient.OnGameEvent += (sender, args) =>
        {
            if (args.Data is null) return;
            Games.Add(new GameViewModel(args.Data));
        };
        _liveClient.OnMoveEvent += (sender, args) =>
        {
            if (args.Data is null) return;
            GameMove move = args.Data;
            GameViewModel game = Games.Where(x => x.GameId == move.GameId).Single();
            game.Moves.Add(new MoveViewModel(move));
        };
    }

    public ObservableCollection<GameViewModel> Games { get; private init; } = new();


    [RelayCommand(IncludeCancelCommand = true)]
    public async Task StartStreamingAsync(CancellationToken token = default)
    {
        await _liveClient.StartAsync(token);
    }
}
