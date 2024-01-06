using Codebreaker.WPF.Contracts.Services.Navigation;

namespace Codebreaker.WPF.Services.Navigation;

internal class PageService(Dictionary<string, Func<Page>> pages, Func<Page> initialPage) : IPageService
{
    public Page GetInitialPage() =>
        initialPage();

    public Page GetPage(string key)
    {
        Func<Page>? pageFactory;

        lock (pages)
            if (!pages.TryGetValue(key, out pageFactory))
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");

        return pageFactory();
    }
}
