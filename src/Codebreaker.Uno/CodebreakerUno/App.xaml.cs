using Codebreaker.GameAPIs.Client;
using Codebreaker.ViewModels;
using Codebreaker.ViewModels.Contracts.Services;
using Codebreaker.ViewModels.Services;
using CodeBreaker.Uno.Services;
using CodeBreaker.Uno.Views.Pages;
//using Uno.Resizetizer;
using CodeBreaker.Uno.Contracts.Services;
using CodeBreaker.Uno.Contracts.Services.Navigation;
using CodeBreaker.Uno.Services.Navigation;
using CodeBreaker.Uno.ViewModels;

namespace CodebreakerUno;

public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    internal Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }
    protected IServiceScope? DefaultScope { get; private set; }
    internal new static App Current => (App)Application.Current;

    public static T GetService<T>()
        where T : class =>
        Current.DefaultScope!.ServiceProvider.GetRequiredService<T>();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ?
                                LogLevel.Information :
                                LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);

                    // Uno Platform namespace filter groups
                    // Uncomment individual methods to see more detailed logging
                    //// Generic Xaml events
                    //logBuilder.XamlLogLevel(LogLevel.Debug);
                    //// Layout specific messages
                    //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                    //// Storage messages
                    //logBuilder.StorageLogLevel(LogLevel.Debug);
                    //// Binding related messages
                    //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                    //// Binder memory references tracking
                    //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                    //// DevServer and HotReload related
                    //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                    //// Debug JS interop
                    //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);

                }, enableUnoLogging: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                )
                // Enable localization (see appsettings.json for supported languages)
                .UseLocalization()
                // Register Json serializers (ISerializer and ISerializer)
                .UseSerialization((context, services) => services
                    .AddContentSerializer(context)
                    // Add Json serialization context if available
                    )
                .UseHttp((context, services) => services
                    // Register HttpClient
#if DEBUG
                    // DelegatingHandler will be automatically injected into Refit Client
                    .AddTransient<DelegatingHandler, DebugHttpHandler>()
#endif
                    )
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IInfoBarService, InfoBarService>();
                    services.AddTransient<INavigationViewService, NavigationViewService>();

                    services.AddTransient<IDialogService, UnoDialogService>();

                    services.AddTransient<ShellPage>();
                    services.AddTransient<ShellViewModel>();

                    services.AddTransient<ISettingsService, SettingsService>();
                    services.AddTransient<SettingsPageViewModel>();

                    string apiBase = context.Configuration["ApiBase"] ?? throw new InvalidOperationException("ApiBase configuration not found");
                    services.AddHttpClient<IGamesClient, GamesClient>((HttpClient client) => client.BaseAddress = new(apiBase));
                    services.AddScoped<GamePageViewModel>();

                    services.AddNavigation(pages => pages
                        .Configure<GamePage>("GamePage")
                        .Configure<SettingsPage>("SettingsPage"));
                })
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.EnableHotReload();
#endif
        //MainWindow.SetWindowIcon();

        Host = builder.Build();
        DefaultScope = Host.Services.CreateScope();

        if (MainWindow.Content is not ShellPage)
        {
            var shell = Host.Services.GetRequiredService<ShellPage>();
            async void OnShellLoaded(object sender, RoutedEventArgs args)
            {
                await Host.Services.GetRequiredService<INavigationService>().NavigateToAsync("GamePage");
                shell.Loaded -= OnShellLoaded;
            }
            shell.Loaded += OnShellLoaded;
            MainWindow.Content = shell;
        }

        // Ensure the current window is active
        MainWindow.Activate();
    }
}
