namespace CodeBreaker.Uno.CustomAttachedProperties;

// Helper class to set the navigation target for a NavigationViewItem.
//
// Usage in XAML:
// <NavigationViewItem x:Uid="Shell_Game" Icon="Document" helpers:Navigation.NavigateTo="AppName.ViewModels.MainViewModel" />
//
// Usage in code:
// Navigation.SetNavigateTo(navigationViewItem, typeof(MainViewModel).FullName);
public partial class NavigationHelper : DependencyObject
{
    public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached(
            "NavigateTo",
            typeof(string),
            typeof(NavigationHelper),
            new PropertyMetadata(null)
        );

    public static string GetNavigateTo(NavigationViewItem item) =>
        (string)item.GetValue(NavigateToProperty);
    
    public static void SetNavigateTo(NavigationViewItem item, string value) =>
        item.SetValue(NavigateToProperty, value);
}
