using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media;

namespace CodeBreaker.WinUI.Helpers;

internal static class PageExtensions
{
    /// <summary>
    /// Registers all message recipients and unregisters them when the page is unloaded.
    /// </summary>
    /// <param name="messenger">The messenger instance.</param>
    /// <param name="page">The page to register and unregister the message recipients.</param>
    public static void RegisterAllAndUnregisterAllOnUnloaded(this IMessenger messenger, FrameworkElement page)
    {
        messenger.RegisterAll(page);
        messenger.UnregisterAllOnUnloaded(page, page);
    }

    /// <summary>
    /// Unregisters all message recipients when the page is unloaded.
    /// </summary>
    /// <param name="messenger">The messenger instance.</param>
    /// <param name="page">The page to unregister the message recipients.</param>
    public static void UnregisterAllOnUnloaded(this IMessenger messenger, FrameworkElement page) =>
        messenger.UnregisterAllOnUnloaded(page, page);

    /// <summary>
    /// Unregisters all message recipients when the page is unloaded.
    /// </summary>
    /// <param name="messenger">The messenger instance.</param>
    /// <param name="page">The page to unregister the message recipients.</param>
    /// <param name="messageRecepient">The specific message recipient to unregister.</param>
    public static void UnregisterAllOnUnloaded(this IMessenger messenger, FrameworkElement page, object messageRecepient)
    {
        void UnloadedCallback(object sender, RoutedEventArgs args)
        {
            messenger.UnregisterAll(messageRecepient);
            page.Unloaded -= UnloadedCallback;
        }

        page.Unloaded += UnloadedCallback;
    }

    /// <summary>
    /// Calls the specified action once when the page is unloaded.
    /// </summary>
    /// <param name="page">The page to call the action on.</param>
    /// <param name="action">The action to be called.</param>
    public static void CallOnceOnUnloaded(this FrameworkElement page, Action<object, RoutedEventArgs> action)
    {
        void Callback(object sender, RoutedEventArgs args)
        {
            action?.Invoke(sender, args);
            page.Unloaded -= Callback;
        }

        page.Unloaded += Callback;
    }

    /// <summary>
    /// Finds all children of the specified type recursively.
    /// </summary>
    /// <typeparam name="T">The type of the children to find.</typeparam>
    /// <param name="dependencyObject">The dependency object to search for children.</param>
    /// <returns>An enumerable collection of children of the specified type.</returns>
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
