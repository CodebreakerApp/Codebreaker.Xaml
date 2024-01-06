
using Codebreaker.ViewModels.Components;

namespace Codebreaker.WPF.Converters;

internal class InfoBarSeverityToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var color = value switch
        {
            InfoMessageSeverity.Error => Colors.Red,
            InfoMessageSeverity.Warning => Colors.Orange,
            InfoMessageSeverity.Info => Colors.Gray,
            InfoMessageSeverity.Success => Colors.Green,
            _ => throw new ArgumentException($"value must be an {nameof(InfoMessageSeverity)}")
        };
        return new SolidColorBrush(color);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
