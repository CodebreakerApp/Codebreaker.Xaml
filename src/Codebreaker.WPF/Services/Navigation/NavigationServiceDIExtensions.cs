using Codebreaker.ViewModels.Contracts.Services;
using Codebreaker.WPF.Contracts.Services.Navigation;
using Codebreaker.WPF.Services.Navigation;

namespace Microsoft.Extensions.DependencyInjection;

internal static class NavigationServiceDIExtensions
{
    public static IServiceCollection AddNavigation<TNavigationService>(this IServiceCollection services, Action<PageServiceBuilder> builder)
        where TNavigationService : class, IWPFNavigationService, INavigationService
    {
        PageServiceBuilder pageServiceBuilder = new();
        builder(pageServiceBuilder);
        services.AddSingleton<IPageService>(pageServiceBuilder.Build());
        services.AddSingleton<IWPFNavigationService, TNavigationService>();
        services.AddSingleton<INavigationService>(x => x.GetRequiredService<IWPFNavigationService>());
        return services;
    }
}
