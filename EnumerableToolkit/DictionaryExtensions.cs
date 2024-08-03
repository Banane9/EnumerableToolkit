namespace EnumerableToolkit
{
    /// <summary>
    /// Contains extension methods for <see cref="IDictionary{TKey, TValue}">dictionaries</see>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Tries to get a value for the given key from this dictionary.<br/>
        /// If the key is not defined, a new instance of <typeparamref name="TValue"/>
        /// will be created using the <paramref name="valueFactory"/>, before being added to the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to get a value out of.</param>
        /// <param name="key">The key to get a value for.</param>
        /// <param name="valueFactory">A factory method that creates a value when there is none yet.</param>
        /// <returns>The value from the dictionary or the newly created and added one.</returns>
        public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            value = valueFactory();
            dictionary.Add(key, value);

            return value;
        }

        /// <summary>
        /// Tries to get a value for the given key from this dictionary.<br/>
        /// If the key is not defined, a new instance of <typeparamref name="TValue"/>
        /// will be created using the default constructor, before being added to the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to get a value out of.</param>
        /// <param name="key">The key to get a value for.</param>
        /// <returns>The value from the dictionary or the newly created and added one.</returns>
        public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            value = new TValue();
            dictionary.Add(key, value);

            return value;
        }
    }
}