using Codebreaker.ViewModels.Contracts.Services;
using Codebreaker.WPF.Services;
using Codebreaker.WPF.Services.Navigation;
using Codebreaker.WPF.Views.Pages;

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
                services.AddNavigation<WPFNavigationService>(pages => pages
                    .Configure<GamePage>("GamePage")
                    .Configure<TestPage>("TestPage")
                    .ConfigureInitialPage<GamePage>());
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

    public static new App Current => (Application.Current as App) ?? throw new InvalidOperationException("The current application is no \"App\"");

    public IServiceScope DefaultScope { get; private init; }

    public IServiceProvider Services => _host.Services;

    public T GetService<T>()
        where T : class =>
        DefaultScope.ServiceProvider.GetRequiredService<T>();

    public void Dispose()
    {
        DefaultScope.Dispose();
        _host.Dispose();
    }
}
