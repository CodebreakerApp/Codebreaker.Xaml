using CodeBreaker.Uno.Contracts.Services.Navigation;
using CodeBreaker.Uno.Views.Pages;

namespace CodeBreaker.Uno.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBackEnabled;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Header))]
    private object? _selected;

    public ShellViewModel(
        IUnoNavigationService navigationService,
        INavigationViewService navigationViewService
    )
    {
        NavigationService = navigationService;
        navigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
    }

    public IUnoNavigationService NavigationService { get; }

    public INavigationViewService NavigationViewService { get; }

    public object? Header => (Selected as ContentControl)?.Content;

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        
        if (selectedItem is not null)
            Selected = selectedItem;
    }
}
