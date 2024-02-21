using Codebreaker.ViewModels;
using System.ComponentModel;

namespace CodeBreaker.WinUI.Views.Components;

public sealed partial class GameResultDisplay : UserControl
{
    public GameResultDisplay()
    {
        InitializeComponent();
        VisualStateManager.GoToState(this, "Default", false);
    }

    public GamePageViewModel ViewModel
    {
        get => (GamePageViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
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
        VisualStateManager.GoToState(this, stateName, true);
    }
}