using Codebreaker.ViewModels.Contracts.Services;
using Codebreaker.WinForms.Services;

namespace Codebreaker.WinForms;

internal static class Program
{
    public static IServiceProvider Services { get; private set; } = default!;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var builder = Host.CreateApplicationBuilder();
        builder.SetDotnetEnvironmentVariable();

        // Register services
        builder.Services.AddSingleton<IInfoBarService, InfoBarService>();
        builder.Services.AddSingleton<IDialogService, WinFormsDialogService>();
        builder.Services.AddScoped<GamePageViewModel>();
        
        builder.Services.AddHttpClient<IGamesClient, GamesClient>(client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetRequired("ApiBase"));
        });

        var host = builder.Build();
        Services = host.Services;

        Application.Run(new MainForm());
    }
}
