using Codebreaker.ViewModels;
using System.ComponentModel;

namespace CodeBreaker.Uno.Views.Components;

public sealed partial class GameResultDisplay : UserControl
{
    public GameResultDisplay()
    {
        InitializeComponent();
        this.GoToState("Default", false);
    }

    public GamePageViewModel ViewModel
    {
        get => (GamePageViewModel)GetValue(MyPropertyProperty); 
        set => SetValue(MyPropertyProperty, value);
    }

    public static readonly DependencyProperty MyPropertyProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(GamePageViewModel), typeof(GameResultDisplay), new PropertyMetadata(null, OnViewModelChanged));

    private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var @this = (GameResultDisplay)d;
        @this.ViewModel.PropertyChanged += @this.OnViewModelPropertyChanged;
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
        this.GoToState(stateName);
    }
}
