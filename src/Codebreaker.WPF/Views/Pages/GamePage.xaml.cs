using Codebreaker.ViewModels.Contracts.Services;

namespace Codebreaker.WPF.Views.Pages;

public partial class GamePage : Page
{
    private readonly INavigationService _navigationService;

    public GamePage()
    {
        ViewModel = App.Current.GetService<GamePageViewModel>();
        _navigationService = App.Current.GetService<INavigationService>();

        DataContext = this;

        InitializeComponent();

        //WeakReferenceMessenger.Default.Register<GameStateChangedMessage>(this, (r, m) =>
        //{
        //    VisualStateManager.GoToElementState(MainGrid, m.GameMode.ToString(), true);
        //});
    }

    public GamePageViewModel ViewModel
    {
        get => (GamePageViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel", typeof(GamePageViewModel), typeof(GamePage), new PropertyMetadata(null));

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await _navigationService.NavigateToAsync("TestPage");
    }
}
