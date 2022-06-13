﻿using CodeBreaker.ViewModels;

using Microsoft.UI.Xaml.Data;

namespace CodeBreaker.WinUI.Converters;

public class GameStatusToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, string? language)
    {
        static Visibility GetStartVisibility(GameMode gameMode) => 
            (gameMode == GameMode.Started || gameMode == GameMode.MoveSet) ? Visibility.Collapsed : Visibility.Visible;

        static Visibility GetRunningVisibility(GameMode gameMode) => 
            (gameMode == GameMode.NotRunning) ? Visibility.Collapsed : Visibility.Visible;

        string uiCategory = parameter?.ToString() ?? throw new InvalidOperationException("Pass a parameter to this converter");


        if (value is GameMode gameMode)
        {
            return uiCategory switch
            {
                "Start" => GetStartVisibility(gameMode),
                "Running" => GetRunningVisibility(gameMode),
                _ => throw new InvalidOperationException("Invalid parameter value")
            };
        }
        else
        {
            throw new InvalidOperationException("GameStatusToVisibilityConverter received an incorrect value type");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
