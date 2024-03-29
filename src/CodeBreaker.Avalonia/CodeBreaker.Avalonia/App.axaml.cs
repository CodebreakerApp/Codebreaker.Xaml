using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Codebreaker.GameAPIs.Client;
using Codebreaker.ViewModels;
using Codebreaker.ViewModels.Contracts.Services;
using Codebreaker.ViewModels.Services;
using CodeBreaker.Avalonia.Services;
using CodeBreaker.Avalonia.Services.Navigation;
using CodeBreaker.Avalonia.Views;
using CodeBreaker.Avalonia.Views.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CodeBreaker.Avalonia;
public partial class App : Application
{
    internal new static App Current => (Application.Current as App) ?? throw new InvalidOperationException("The current application is no \"App\"");

    public static T GetService<T>()
        where T : class =>
        Current.DefaultScope.ServiceProvider.GetRequiredService<T>();

    public App()
    {
        this.SetDotnetEnvironmentVariable();
        var builder = Host.CreateApplicationBuilder(Environment.GetCommandLineArgs());

        // Configuration
        builder.Configuration.AddAppSettingsJson();

        // Services
        builder.Services.AddNavigation<AvaloniaNavigationService>(pages => pages
            .Configure<GamePage>("GamePage")
            .Configure<TestPage>("TestPage")
            .ConfigureInitialPage("GamePage"));
        builder.Services.Configure<GamePageViewModelOptions>(options => { });
        builder.Services.AddScoped<IInfoBarService, InfoBarService>();
        builder.Services.AddTransient<IDialogService, AvaloniaDialogService>();
        builder.Services.AddScoped<GamePageViewModel>();
        builder.Services.AddHttpClient<IGamesClient, GamesClient>(client =>
        {
            client.BaseAddress = new(builder.Configuration.GetRequired("ApiBase"));
        });
        var host = builder.Build();
        DefaultScope = host.Services.CreateScope();
    }

    internal IServiceScope DefaultScope { get; private set; }

    public override void Initialize() =>
        AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow()
            {
                Content = new Shell()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new Shell();
        }

        base.OnFrameworkInitializationCompleted();
    }
}