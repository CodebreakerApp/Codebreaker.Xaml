using Avalonia.Controls;
using Avalonia.Interactivity;
using Codebreaker.ViewModels;
using Codebreaker.ViewModels.Contracts.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace CodeBreaker.Avalonia.Views.Pages;

public partial class GamePage : UserControl, IRecipient<GameMoveMessage>
{
    private readonly INavigationService _navigationService;

    public GamePage()
    {
        DataContext = App.GetService<GamePageViewModel>();
        _navigationService = App.GetService<INavigationService>();
        InitializeComponent();
        WeakReferenceMessenger.Default.Register(this);
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        Classes.Add("start");
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public GamePageViewModel ViewModel => (GamePageViewModel)DataContext!;

    public void Receive(GameMoveMessage message)
    {
        if (message.GameMoveValue is not GameMoveValue.Completed)
            return;

        PegScrollViewer.UpdateLayout();
        PegScrollViewer.ScrollToEnd();
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(GamePageViewModel.GameStatus))
            return;

        var stateName = ViewModel.GameStatus switch
        {
            GameMode.Started or GameMode.MoveSet => "playing",
            GameMode.Won or GameMode.Lost => "finished",
            _ => "start",
        };
        Classes.Clear();
        Classes.Add(stateName);
    }

    private void ToTestPageButtonClicked(object? sender, RoutedEventArgs e)
    {
        _navigationService.NavigateToAsync("TestPage");
    }
}