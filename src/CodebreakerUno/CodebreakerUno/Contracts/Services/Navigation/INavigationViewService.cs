namespace CodeBreaker.Uno.Contracts.Services.Navigation;

public interface INavigationViewService
{
    IList<object>? MenuItems { get; }
    object? SettingsItem { get; }

    NavigationViewItem? GetSelectedItem(Type pageType);
    void Initialize(NavigationView navigationView);
    void UnregisterEvents();
}
