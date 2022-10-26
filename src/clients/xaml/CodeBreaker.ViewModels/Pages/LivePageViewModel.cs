﻿using System.Collections.ObjectModel;
using CodeBreaker.Services;
using CodeBreaker.ViewModels.Components;
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
            if (args.Data?.Game is null) return;
            Games.Add(new GameViewModel(args.Data.Game));
        };
        _liveClient.OnMoveEvent += (sender, args) =>
        {
            if (args.Data?.Move is null) return;
            GameViewModel? game = Games.Where(x => x.GameId == args.Data.GameId).SingleOrDefault();
            game?.Moves.Add(new MoveViewModel(args.Data.Move));
        };
    }

    public ObservableCollection<GameViewModel> Games { get; private init; } = new();


    [RelayCommand(AllowConcurrentExecutions = false, FlowExceptionsToTaskScheduler = true, IncludeCancelCommand = true)]
    public async Task StartStreamingAsync(CancellationToken token = default)
    {
        await _liveClient.StartAsync(token);
    }
}
