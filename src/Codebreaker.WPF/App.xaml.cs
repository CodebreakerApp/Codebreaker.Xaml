﻿using Codebreaker.ViewModels.Contracts.Services;
using Codebreaker.WPF.Contracts;
using Codebreaker.WPF.Services;

namespace Codebreaker.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public sealed partial class App : Application, IDisposable
{
    private readonly IHost _host;

    public App()
    {
#if DEBUG
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
#else
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Production");
#endif

        _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(options =>
            {
                string? test = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<GamePageViewModelOptions>(options => { });
                services.AddSingleton<IWPFNavigationService, WPFNavigationService>();
                services.AddSingleton<INavigationService>(x => x.GetRequiredService<IWPFNavigationService>());
                services.AddTransient<IDialogService, WPFDialogService>();
                services.AddSingleton<IInfoBarService, InfoBarService>();
                services.AddScoped<GamePageViewModel>();
                services.AddHttpClient<IGamesClient, GamesClient>(client =>
                {
                    string uriString = context.Configuration["ApiBase"] ?? throw new ConfigurationErrorsException("ApiBase not configured");
                    client.BaseAddress = new Uri(uriString);
                });
            })
            .Build();

        DefaultScope = _host.Services.CreateScope();
    }

    public void Dispose()
    {
        DefaultScope.Dispose();
        _host.Dispose();
    }

    public IServiceScope DefaultScope { get; private init; }

    public IServiceProvider Services => _host.Services;

    public T GetService<T>()
        where T : class =>
        DefaultScope.ServiceProvider.GetRequiredService<T>();

    protected override void OnActivated(EventArgs e)
    protected override async void OnActivated(EventArgs e)
    {
        base.OnActivated(e);
        await GetService<IWPFNavigationService>().NavigateToAsync("GamePage");
    }
}
