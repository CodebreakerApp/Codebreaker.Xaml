namespace CodeBreaker.Uno.Services.Navigation;

internal class PageServiceBuilder
{
    private readonly Dictionary<string, Type> _pages = [];

    public PageServiceBuilder Configure<V>()
        where V : Page =>
        Configure<V>(typeof(V).Name);

    public PageServiceBuilder Configure<V>(string key)
        where V : Page
    {
        lock (_pages)
        {
            if (_pages.ContainsKey(key))
                throw new ArgumentException($"The key {key} is already configured in {nameof(PageService)}");

            var pageType = typeof(V);

            if (_pages.ContainsValue(pageType))
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == pageType).Key}");

            _pages.Add(key, pageType);
        }

        return this;
    }

    public PageService Build() =>
        new(_pages);
}
