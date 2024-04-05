namespace Codebreaker.ViewModels.Services;

[Obsolete("Use IInfoBarService instead.")]
public interface IDialogService
{
    Task ShowMessageAsync(string message);
}
