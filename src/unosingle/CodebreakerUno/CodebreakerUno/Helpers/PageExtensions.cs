using CommunityToolkit.Mvvm.Messaging;

namespace CodeBreaker.Uno.Helpers;

internal static class PageExtensions
{
    public static void UnregisterAllOnUnloaded(this IMessenger messenger, FrameworkElement page) =>
        messenger.UnregisterAllOnUnloaded(page, page);

    public static void UnregisterAllOnUnloaded(this IMessenger messenger, FrameworkElement page, object messageRecepient)
    {
        void UnloadedCallback(object sender, RoutedEventArgs args)
        {
            messenger.UnregisterAll(messageRecepient);
            page.Unloaded -= UnloadedCallback;
        }

        page.Unloaded += UnloadedCallback;
    }

    public static void CallOnceOnUnloaded(this FrameworkElement page, Action<object, RoutedEventArgs> action)
    {
        void Callback(object sender, RoutedEventArgs args)
        {
            action?.Invoke(sender, args);
            page.Unloaded -= Callback;
        }

        page.Unloaded += Callback;
    }

    public static IEnumerable<T> FindItemsOfType<T>(this DependencyObject dependencyObject, DependencyObject obj)
        where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);

            if (child is null)
                yield break;

            if (child is T item)
                yield return item;

            foreach (T childOfChild in dependencyObject.FindItemsOfType<T>(child))
                yield return childOfChild;
        }
    }
}
