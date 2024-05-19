using Codebreaker.ViewModels.Contracts.Services;
using CodeBreaker.Uno.Contracts.Services.Navigation;
using CodeBreaker.Uno.Services.Navigation;

namespace Microsoft.Extensions.DependencyInjection;

internal static class PageServiceDIExtensions
{
    public static IServiceCollection AddPageService(this IServiceCollection services, Action<PageServiceBuilder> builder)
    {
        PageServiceBuilder pageServiceBuilder = new();
        builder(pageServiceBuilder);
        services.AddSingleton<IPageService>(pageServiceBuilder.Build());
        return services;
    }

    public static IServiceCollection AddNavigation(this IServiceCollection services, Action<PageServiceBuilder> builder)
    {
        services.AddPageService(builder);
        services.AddScoped<IUnoNavigationService, UnoNavigationService>();
        services.AddScoped<INavigationService>(x => x.GetRequiredService<IUnoNavigationService>());
        return services;
    }
}
