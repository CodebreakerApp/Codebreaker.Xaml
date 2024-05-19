namespace CodeBreaker.Uno.Contracts.Services.Navigation;

internal interface IPageService
{
    /// <summary>
    /// Get the page type by specifying the corresponding key.
    /// </summary>
    /// <param name="key">The key for the page.</param>
    /// <returns>The page type.</returns>
    /// <exception cref="ArgumentException">Thrown if the page type is not found.</exception>
    Type GetPageType(string key);

    Type this[string key] => GetPageType(key);
}
