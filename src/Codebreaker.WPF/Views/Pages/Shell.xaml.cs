using Codebreaker.WPF.Contracts.Services.Navigation;

namespace Codebreaker.WPF.Views.Pages;

/// <summary>
/// Interaction logic for Shell.xaml
/// </summary>
public partial class Shell : Page
{
    private readonly IWPFNavigationService _navigationService;

    public Shell()
    {
        InitializeComponent();
        _navigationService = App.Current.GetService<IWPFNavigationService>();
        _navigationService.Frame = NavigationFrame;
    }
}
