using System.ComponentModel;

namespace Codebreaker.MAUI.Views.Components;

public partial class GameResultDisplay : ContentView
{
	public GameResultDisplay()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        GoToState("Default");
    }

    public GamePageViewModel ViewModel => (GamePageViewModel)BindingContext;

    private void OnLoaded(object? sender, EventArgs e)
    {
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != nameof(GamePageViewModel.GameStatus))
            return;

        var stateName = ViewModel.GameStatus switch
        {
            GameMode.Won => "Won",
            GameMode.Lost => "Lost",
            _ => "Default"
        };
        GoToState(stateName);
    }

    private void GoToState(string stateName) =>
        VisualStateManager.GoToState(this, stateName);
}