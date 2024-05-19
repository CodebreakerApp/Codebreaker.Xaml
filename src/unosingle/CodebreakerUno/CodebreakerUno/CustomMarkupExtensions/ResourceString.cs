using CodeBreaker.Uno.Helpers;
using Microsoft.UI.Xaml.Markup;

namespace CodeBreaker.Uno.CustomMarkupExtensions;

[MarkupExtensionReturnType(ReturnType = typeof(string))]
internal class ResourceString : MarkupExtension
{
    public string? Name { get; set; }

    protected override object ProvideValue() =>
        string.IsNullOrEmpty(Name)
        ? string.Empty
        : Name.GetLocalized();
}
