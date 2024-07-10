namespace System.Collections.Generic;

internal static class DictionaryExtensions
{
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = default) =>
        dictionary.TryGetValue(key, out var value) ? value : defaultValue;
}
