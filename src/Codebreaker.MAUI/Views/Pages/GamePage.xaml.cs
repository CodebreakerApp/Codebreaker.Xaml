using Codebreaker.ViewModels.Contracts.Services;

namespace Codebreaker.MAUI.Views.Pages;

public partial class GamePage : ContentPage, IRecipient<GameMoveMessage>, IRecipient<InfoMessage>
{
	private readonly INavigationService _navigationService;

	public GamePage(GamePageViewModel viewModel, INavigationService navigationService)
	{
		_navigationService = navigationService;
		ViewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
        WeakReferenceMessenger.Default.RegisterAll(this);
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        this.GoToState("Start");
    }

    public GamePageViewModel ViewModel { get; }

    public async void Receive(GameMoveMessage message)
    {
		if (message.GameMoveValue is not GameMoveValue.Completed)
			return;

        await Task.Delay(300);
        await PegScrollView.ScrollToAsync(0, PegScrollView.ContentSize.Height, true);
    }

    public async void Receive(InfoMessage message)
    {
        await DisplayAlert("Info", message.Text, "Close");
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(GamePageViewModel.GameStatus))
            return;

        var stateName = ViewModel.GameStatus switch
        {
            GameMode.Started or GameMode.MoveSet => "Playing",
            GameMode.Won or GameMode.Lost => "Finished",
            _ => "Start",
        };
        this.GoToState(stateName);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await _navigationService.NavigateToAsync("TestPage");
    }
}
