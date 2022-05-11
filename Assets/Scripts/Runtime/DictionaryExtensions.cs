using System;
using System.Collections.Generic;

namespace ExtensionsPack
{
	public static class DictionaryExtensions
	{
		public static bool TryGetValueOrDo<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value,
			Action action)
		{
			if (dictionary.TryGetValue(key, out value))
				return true;

			action?.Invoke();
			return false;
		}

		public static void ReplaceKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey fromKey, TKey toKey)
		{
			var value = dictionary[fromKey];
			dictionary.Remove(fromKey);
			dictionary[toKey] = value;
		}
	}
}