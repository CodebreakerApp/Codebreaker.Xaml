using CodeBreaker.Uno.Contracts.Services;
using CodeBreaker.Uno.Helpers;

namespace CodeBreaker.Uno.ViewModels;

public partial class SettingsPageViewModel(ISettingsService settingsService) : ObservableObject
{
    public partial record class LanguageSelectionItem(string Key, string Text);

    [ObservableProperty]
    private bool _languageChangeInfoBarVisible = false;

    public LanguageSelectionItem[] LanguageSelections { get; private init; } =
        [
            new (string.Empty, "Settings_Language_Default".GetLocalized()),
            new ("de", "Settings_Language_German".GetLocalized()),
            new ("en-US", "Settings_Language_English".GetLocalized())
        ];

    public ElementTheme Theme
    {
        get => settingsService.Theme;
        set
        {
            if (Theme == value)
                return;

            OnPropertyChanging(nameof(Theme));
            settingsService.Theme = value;
            OnPropertyChanged(nameof(Theme));
        }
    }

    public string LanguageKey
    {
        get => settingsService.LanguageKey;
        set
        {
            if (LanguageKey == value)
                return;

            OnPropertyChanging(nameof(LanguageKey));
            settingsService.LanguageKey = value;
            OnPropertyChanged(nameof(LanguageKey));
            LanguageChangeInfoBarVisible = true;
        }
    }
}
