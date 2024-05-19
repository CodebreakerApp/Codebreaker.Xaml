using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Windows.UI;

namespace CodeBreaker.Uno.Converters;

public class ColornameToBrushConverter : IValueConverter
{
    private readonly static Brush s_blackBrush = new SolidColorBrush(Colors.Black);
    private readonly static Brush s_whiteBrush = new SolidColorBrush(Colors.White);
    private readonly static Brush s_redBrush = new SolidColorBrush(Color.FromArgb(255, 209, 52, 56));
    private readonly static Brush s_greenBrush = new SolidColorBrush(Color.FromArgb(255, 0, 173, 86));
    private readonly static Brush s_blueBrush = new SolidColorBrush(Color.FromArgb(255, 0, 128, 212));
    private readonly static Brush s_yellowBrush = new SolidColorBrush(Color.FromArgb(255, 252, 225, 0));
    private readonly static Brush s_orangeBrush = new SolidColorBrush(Color.FromArgb(255, 234, 74, 33));
    private readonly static Brush s_purpleBrush = new SolidColorBrush(Color.FromArgb(255, 91, 95, 199));
    private readonly static Brush s_emptyBrush = new SolidColorBrush(Color.FromArgb(255, 160, 174, 178));

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        if (value is not string colorname)
            throw new ArgumentException("Value is no string");

        return colorname switch
        {
            "Black" => s_blackBrush,
            "White" => s_whiteBrush,
            "Red" => s_redBrush,
            "Green" => s_greenBrush,
            "Blue" => s_blueBrush,
            "Yellow" => s_yellowBrush,
            "Orange" => s_orangeBrush,
            "Purple" => s_purpleBrush,
            _ => s_emptyBrush
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
