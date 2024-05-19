using Windows.ApplicationModel.Resources;

namespace CodeBreaker.Uno.Helpers;

internal static class ResourceExtensions
{
    private static readonly ResourceLoader s_resourceLoader = ResourceLoader.GetForViewIndependentUse();

    public static string GetLocalized(this string resourceKey) => s_resourceLoader.GetString(resourceKey);
}
