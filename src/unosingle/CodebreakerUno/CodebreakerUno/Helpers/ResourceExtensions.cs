namespace CodeBreaker.Uno.Helpers;

internal static class ResourceExtensions
{
    private static readonly IStringLocalizer s_stringLocalizer = App.GetService<IStringLocalizer>();

    public static string GetLocalized(this string resourceKey) => s_stringLocalizer.GetString(resourceKey);
}
