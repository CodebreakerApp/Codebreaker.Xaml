using Codebreaker.ViewModels.Contracts.Services;

namespace CodeBreaker.WinUI.Views.Components.General;

internal sealed partial class InfoBarArea : UserControl
{
    public InfoBarArea()
    {
        ViewModel = App.GetService<IInfoBarService>();
        InitializeComponent();
    }

    public IInfoBarService ViewModel { get; }
}
