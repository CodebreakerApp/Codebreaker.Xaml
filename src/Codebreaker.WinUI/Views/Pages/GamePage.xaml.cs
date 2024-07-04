using Codebreaker.ViewModels;
using Codebreaker.ViewModels.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace CodeBreaker.WinUI.Views.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class GamePage : Page,
    IRecipient<GameStartedMessage>,
    IRecipient<GameEndedMessage>,
    IRecipient<GameCancelledMessage>
{
    public GamePageViewModel ViewModel { get; }

    public GamePage()
    {
        ViewModel = App.GetService<GamePageViewModel>();
        WeakReferenceMessenger.Default.RegisterAllAndUnregisterAllOnUnloaded(this);
        InitializeComponent();
        this.GoToState("Start", false);
    }

    public void Receive(GameStartedMessage message) =>
        this.GoToState("Playing");

    public async void Receive(GameEndedMessage message)
    {
        this.GoToState("Finished");
        await Task.Delay(1000);
    }

    public void Receive(GameCancelledMessage message) =>
        this.GoToState("Start");
}
