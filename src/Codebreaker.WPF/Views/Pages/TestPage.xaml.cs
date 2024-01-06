using Codebreaker.ViewModels.Contracts.Services;

namespace Codebreaker.WPF.Views.Pages;

/// <summary>
/// Interaction logic for TestPage.xaml
/// </summary>
public partial class TestPage : Page
{
    private readonly INavigationService _navigationService;
    
    public TestPage()
    {
        InitializeComponent();
        _navigationService = App.Current.GetService<INavigationService>();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await _navigationService.NavigateToAsync("GamePage");
    }
}
