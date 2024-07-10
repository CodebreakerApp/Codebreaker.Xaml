namespace Codebreaker.ViewModels.Models.Extensions;

internal static class FieldExtensions
{
    public static IEnumerable<string> Serialize(this IEnumerable<Field> fields) =>
        fields.Select(f => f.Serialize());
}
