using Codebreaker.ViewModels.Contracts.Services;
using Codebreaker.WPF.Helpers;
using CommunityToolkit.Mvvm.Messaging;

namespace Codebreaker.WPF.Views.Pages;

public partial class GamePage : Page, IRecipient<GameMoveMessage>
{
    private readonly INavigationService _navigationService;

    public GamePage()
    {
        ViewModel = App.GetService<GamePageViewModel>();
        _navigationService = App.GetService<INavigationService>();
        DataContext = this;
        InitializeComponent();
        WeakReferenceMessenger.Default.Register(this);
        WeakReferenceMessenger.Default.UnregisterAllOnUnloaded(this);
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        ContentWrapper.GoToState("Start");
    }

    public GamePageViewModel ViewModel { get; }

    public void Receive(GameMoveMessage message)
    {
        if (message.GameMoveValue is not GameMoveValue.Completed)
            return;

        PegScrollViewer.UpdateLayout();
        PegScrollViewer.ScrollToBottom();
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(GamePageViewModel.GameStatus))
            return;

        var stateName = ViewModel.GameStatus switch
        {
            GameMode.Started or GameMode.MoveSet => "Playing",
            GameMode.Won or GameMode.Lost => "Finished",
            _ => "Start"
        };
        ContentWrapper.GoToState(stateName);
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await _navigationService.NavigateToAsync("TestPage");
    }
}
