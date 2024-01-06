using Codebreaker.ViewModels.Contracts.Services;

namespace Codebreaker.WPF.Views.Components;

/// <summary>
/// Interaction logic for InfoBarArea.xaml
/// </summary>
public partial class InfoBarArea : UserControl
{
    public InfoBarArea()
    {
        ViewModel = App.Current.GetService<IInfoBarService>();
        InitializeComponent();
        DataContext = this;
    }

    public IInfoBarService ViewModel { get; }
}
