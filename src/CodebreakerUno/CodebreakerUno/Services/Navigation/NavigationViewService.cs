using CodeBreaker.Uno.Contracts.Services.Navigation;
using CodeBreaker.Uno.CustomAttachedProperties;
using CodeBreaker.Uno.Views.Pages;

namespace CodeBreaker.Uno.Services.Navigation;

internal class NavigationViewService(IUnoNavigationService navigationService, IPageService pageService) : INavigationViewService
{
    private NavigationView? _navigationView;

    public IList<object>? MenuItems => _navigationView?.MenuItems;

    public object? SettingsItem => _navigationView?.SettingsItem;

    public void Initialize(NavigationView navigationView)
    {
        _navigationView = navigationView;
        _navigationView.BackRequested += OnBackRequested;
        _navigationView.ItemInvoked += OnItemInvoked;
    }

    public void UnregisterEvents()
    {
        if (_navigationView is not null)
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }
    }

    public NavigationViewItem? GetSelectedItem(Type pageType)
    {
        IEnumerable<object>? menuItems = _navigationView?.MenuItems;
        IEnumerable<object>? footerMenuItems = _navigationView?.FooterMenuItems;
        IEnumerable<object>? allMenuItems = footerMenuItems is null
            ? menuItems
        : menuItems?.Concat(footerMenuItems);
        return GetSelectedItem(allMenuItems, pageType);
    }

    private async void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => await navigationService.GoBackAsync();

    private async void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked)
        {
            await navigationService.NavigateToAsync(nameof(SettingsPage));
        }
        else
        {
            if (args.InvokedItemContainer is not NavigationViewItem selectedItem)
                return;

            if (selectedItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
                await navigationService.NavigateToAsync(pageKey);
        }
    }

    private NavigationViewItem? GetSelectedItem(IEnumerable<object>? menuItems, Type pageType)
    {
        if (menuItems is null)
            return null;

        foreach (var item in menuItems.OfType<NavigationViewItem>())
        {
            if (IsMenuItemForPageType(item, pageType))
                return item;

            NavigationViewItem? selectedChild = GetSelectedItem(item.MenuItems, pageType);

            if (selectedChild != null)
                return selectedChild;
        }

        return null;
    }

    private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
    {
        if (menuItem.GetValue(NavigationHelper.NavigateToProperty) is string pageKey)
            return pageService.GetPageType(pageKey) == sourcePageType;

        return false;
    }
}
