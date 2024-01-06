namespace Codebreaker.WPF.Contracts.Services.Navigation;

internal interface IPageService
{
    Page GetPage(string key);

    Page GetInitialPage();

    public Page this[string key] => GetPage(key);
}
