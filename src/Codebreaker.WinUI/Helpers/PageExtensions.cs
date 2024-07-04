using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media;

namespace CodeBreaker.WinUI.Helpers;

internal static class PageExtensions
{
    public static void RegisterAllAndUnregisterAllOnUnloaded(this IMessenger messenger, FrameworkElement page)
    {
        messenger.RegisterAll(page);
        messenger.UnregisterAllOnUnloaded(page, page);
    }
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

    public static IEnumerable<T> FindChildrenRecursively<T>(this DependencyObject dependencyObject)
        where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);

            if (child is null)
                yield break;

            if (child is T item)
                yield return item;

            foreach (T childOfChild in child.FindChildrenRecursively<T>())
                yield return childOfChild;
        }
    }
}
