using CodeBreaker.Uno.Contracts.Services.Navigation;

namespace CodeBreaker.Uno.Services.Navigation;

internal class PageService(Dictionary<string, Type> pages) : IPageService
{
    private readonly Dictionary<string, Type> _pages = pages;

    /// <summary>
    /// Get the page type by specifying the corresponding key.
    /// </summary>
    /// <param name="key">The key for the page.</param>
    /// <returns>The page type.</returns>
    /// <exception cref="ArgumentException">Thrown if the page type was not found.</exception>
    public Type GetPageType(string key)
    {
        Type? pageType;

        lock (_pages)
            if (!_pages.TryGetValue(key, out pageType))
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");

        return pageType;
    }
}
