﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using static CodeBreaker.Shared.CodeBreakerColors;

namespace CodeBreaker.WPF.Converters;

public class PegColorToBrushConverter : IValueConverter
{
    private static readonly Brush s_blackBrush = new SolidColorBrush(Colors.Black);
    private static readonly Brush s_whiteBrush = new SolidColorBrush(Colors.White);
    private static readonly Brush s_redBrush = new SolidColorBrush(Colors.Red);
    private static readonly Brush s_greenBrush = new SolidColorBrush(Colors.Green);
    private static readonly Brush s_blueBrush = new SolidColorBrush(Colors.Blue);
    private static readonly Brush s_yellowBrush = new SolidColorBrush(Colors.Yellow);
    private static readonly Brush s_emptyBrush = new SolidColorBrush(Colors.LightGray);

    public Brush BlackBrush { get; set; } = s_blackBrush;
    public Brush WhiteBrush { get; set; } = s_whiteBrush;
    public Brush RedBrush { get; set; } = s_redBrush;
    public Brush GreenBrush { get; set; } = s_greenBrush;
    public Brush BlueBrush { get; set; } = s_blueBrush;
    public Brush YellowBrush { get; set; } = s_yellowBrush;

    private Brush EmptyBrush { get; set; } = s_emptyBrush;

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            Black => BlackBrush,
            White => WhiteBrush,
            Red => RedBrush,
            Green => GreenBrush,
            Blue => BlueBrush,
            Yellow => YellowBrush,
            _ => EmptyBrush
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
