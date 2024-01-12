
namespace CodebreakerUno.Contracts.Services;

public interface ISettingsService
{
    string LanguageKey { get; set; }
    ElementTheme Theme { get; set; }

    bool TrySettingStoredTheme();
}
