namespace Codebreaker.WPF.Services.Navigation;

internal class PageServiceBuilder
{
    private readonly Dictionary<string, Func<Page>> _pages = new();

    private Func<Page>? _initialPage;

    public PageServiceBuilder Configure<TView>()
        where TView : Page, new() =>
        Configure<TView>(typeof(TView).Name);

    public PageServiceBuilder Configure<TView>(string key)
        where TView : Page, new()
    {
        lock (_pages)
        {
            if (_pages.ContainsKey(key))
                throw new ArgumentException($"The key {key} is already configured in {nameof(PageService)}");

            var pageFactory = () => new TView();

            if (_pages.ContainsValue(pageFactory))
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == pageFactory).Key}");

            _pages.Add(key, pageFactory);
        }

        return this;
    }

    public PageServiceBuilder ConfigureInitialPage<TView>()
        where TView : Page, new()
    {
        _initialPage = () => new TView();
        return this;
    }

    public PageService Build()
    {
        if (_initialPage is null)
            throw new ArgumentNullException(nameof(_initialPage), $"The initial page must be configured. Did you call .{nameof(ConfigureInitialPage)}?");

        return new(_pages, _initialPage);
    }
}