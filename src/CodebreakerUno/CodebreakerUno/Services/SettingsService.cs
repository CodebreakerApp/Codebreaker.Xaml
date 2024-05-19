using CodeBreaker.Uno.Contracts.Services;
using Windows.Foundation.Collections;
using Windows.Globalization;

namespace CodeBreaker.Uno.Services;

public class SettingsService : ISettingsService
{
    private static IPropertySet LocalSettings => ApplicationData.Current.LocalSettings.Values; // ApplicationData not support for Windows Unpackaged build

    public ElementTheme Theme
    {
        get
        {
            if (LocalSettings["Theme"] is int storedElementTheme && Enum.IsDefined((ElementTheme)storedElementTheme))
                return (ElementTheme)storedElementTheme;

            if (App.Current.MainWindow?.Content is FrameworkElement rootElement)
                return rootElement.RequestedTheme;

            return ElementTheme.Default;
        }
        set
        {
            if (!Enum.IsDefined(value))
                return;

            if (Theme == value)
                return;

            if (App.Current.MainWindow?.Content is not FrameworkElement rootElement)
                return;

            rootElement.RequestedTheme = value;
            //TitleBarHelper.UpdateTitleBar(value);
            LocalSettings["Theme"] = (int)value;
        }
    }

    public string LanguageKey
    {
        get => ApplicationLanguages.PrimaryLanguageOverride;
        set
        {
            if (LanguageKey == value)
                return;

            ApplicationLanguages.PrimaryLanguageOverride = value;   // Set language
            //Frame.BackStack.Clear();                                // Clear cached paged with old language
            //Frame.Navigate(typeof(SettingsPage));                   // Reload the page
        }
    }

    public bool TrySettingStoredTheme()
    {
        if (App.Current.MainWindow?.Content is not FrameworkElement rootElement)
            return false;

        rootElement.RequestedTheme = Theme;
        return true;
    }
}
