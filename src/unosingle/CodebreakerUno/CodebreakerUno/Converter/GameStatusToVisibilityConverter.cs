using Codebreaker.ViewModels;
using Microsoft.UI.Xaml.Data;

namespace CodeBreaker.Uno.Converters;

internal class GameStatusToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, string? language)
    {
        if (value is not GameMode gameMode)
            throw new InvalidOperationException("GameStatusToVisibilityConverter received an incorrect value type");

        static Visibility GetStartVisibility(GameMode gameMode) =>
        gameMode is not GameMode.NotRunning ? Visibility.Collapsed : Visibility.Visible;

        static Visibility GetRunningVisibility(GameMode gameMode) =>
            gameMode is GameMode.NotRunning ? Visibility.Collapsed : Visibility.Visible;

        static Visibility GetCancelVisibility(GameMode gameMode) =>
            gameMode is GameMode.Started or GameMode.MoveSet ? Visibility.Visible : Visibility.Collapsed;

        static Visibility GetLostVisibility(GameMode gameMode) =>
            gameMode is GameMode.Lost ? Visibility.Visible : Visibility.Collapsed;

        static Visibility GetWonVisibility(GameMode gameMode) =>
            gameMode is GameMode.Won ? Visibility.Visible : Visibility.Collapsed;

        string uiCategory = parameter?.ToString() ?? throw new InvalidOperationException("Pass a parameter to this converter");

        return uiCategory switch
        {
            "Start" => GetStartVisibility(gameMode),
            "Running" => GetRunningVisibility(gameMode),
            "Cancelable" => GetCancelVisibility(gameMode),
            "Lost" => GetLostVisibility(gameMode),
            "Won" => GetWonVisibility(gameMode),
            _ => throw new InvalidOperationException("Invalid parameter value")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotImplementedException();
}
