using Codebreaker.ViewModels;
using Codebreaker.ViewModels.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;

namespace CodeBreaker.WinUI.Views.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class GamePage : Page,
    IRecipient<MakeMoveMessage>,
    IRecipient<GameStartedMessage>,
    IRecipient<GameEndedMessage>,
    IRecipient<GameCancelledMessage>
{
    public GamePageViewModel ViewModel { get; }

    public GamePage()
    {
        ViewModel = App.GetService<GamePageViewModel>();
        WeakReferenceMessenger.Default.RegisterAll(this);
        WeakReferenceMessenger.Default.UnregisterAllOnUnloaded(this);
        InitializeComponent();
        this.GoToState("Start", false);
    }

    public void Receive(GameStartedMessage message) =>
        this.GoToState("Playing");

    public async void Receive(GameEndedMessage message)
    {
        this.GoToState("Finished");
        await Task.Delay(1000);
        ScrollPegListToBottom();
    }

    public void Receive(MakeMoveMessage message)
    {
        // Move must be completed
        if (!message.IsSet)
            return;

        var animationService = ConnectedAnimationService.GetForCurrentView();
        animationService.DefaultDuration = TimeSpan.FromMilliseconds(500);
        listGameMoves
            .ItemContainerGenerator
            .ContainerFromIndex(listGameMoves.Items.Count - 1)
            .FindChildrenRecursively<Ellipse>()
            .Foreach((ellipse, i) =>
            {
                ConnectedAnimation? animation = animationService.GetAnimation($"guess{i}");

                // No animation found for this ellipxe -> the ellipse is most likely a key-peg
                if (animation is null)
                    return;

                animation.Configuration = new BasicConnectedAnimationConfiguration();
                animation.TryStart(ellipse);
            });

        ScrollPegListToBottom();
    }

    private void ScrollPegListToBottom()
    {
        PegScrollViewer.UpdateLayout();
        PegScrollViewer.ChangeView(null, PegScrollViewer.ScrollableHeight, null, false);
    }

    public void Receive(GameCancelledMessage message)
    {
        this.GoToState("Start");
    }
}
