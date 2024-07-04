using Codebreaker.ViewModels;
using Codebreaker.ViewModels.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;

namespace CodeBreaker.WinUI.Views.Components.GamePage
{
    public sealed partial class Moves : UserControl,
        IRecipient<GameEndedMessage>,
        IRecipient<MakeMoveMessage>
    {
        public Moves()
        {
            WeakReferenceMessenger.Default.RegisterAllAndUnregisterAllOnUnloaded(this);
            InitializeComponent();
        }

        // Dependency property for the ViewModel
        public GamePageViewModel ViewModel
        {
            get => (GamePageViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(GamePageViewModel), typeof(StartGameComponent), new PropertyMetadata(null));

        public async void Receive(GameEndedMessage message)
        {
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
            ScrollViewer.UpdateLayout();
            ScrollViewer.ChangeView(null, ScrollViewer.ScrollableHeight, null, false);
        }
    }
}
