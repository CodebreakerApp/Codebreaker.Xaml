using Codebreaker.ViewModels.Contracts.Services;

namespace Codebreaker.WinForms.Services;

/// <summary>
/// Service for displaying dialog messages in WinForms applications.
/// </summary>
public class WinFormsDialogService : IDialogService
{
    public Task ShowMessageAsync(string title, string message)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        return Task.CompletedTask;
    }

    public Task ShowErrorAsync(string title, string message)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return Task.CompletedTask;
    }
}
