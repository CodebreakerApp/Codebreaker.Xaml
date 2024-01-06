
namespace Codebreaker.WPF.Converters;

internal class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed,
            _ => throw new ArgumentException("Value must be a boolean")
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            Visibility.Visible => true,
            Visibility.Hidden => false,
            Visibility.Collapsed => false,
            _ => throw new ArgumentException("Value must be a visibility")
        };
}
