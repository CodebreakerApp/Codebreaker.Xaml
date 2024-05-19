using CodeBreaker.Uno.ViewModels;

namespace CodeBreaker.Uno.Views.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsPageViewModel>();
        InitializeComponent();
    }

    public SettingsPageViewModel ViewModel { get; }
}
