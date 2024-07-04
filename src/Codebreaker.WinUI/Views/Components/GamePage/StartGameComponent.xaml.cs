using Codebreaker.ViewModels;

namespace CodeBreaker.WinUI.Views.Components.GamePage;

public sealed partial class StartGameComponent : UserControl
{
    public StartGameComponent()
    {
        InitializeComponent();
    }

    // Dependency property for the ViewModel
    public GamePageViewModel ViewModel
    {
        get => (GamePageViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(GamePageViewModel), typeof(StartGameComponent), new PropertyMetadata(null));
}
