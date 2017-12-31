using System;
using System.Collections.Generic;
using System.Linq;

namespace ExistAll.DataStore.Collections
{
	public static class DictionaryExtensions
	{
		public static TValue ItemOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> target,
			TKey key,
			TValue @default = default(TValue))
		{
			TValue value;
			return target.TryGetValue(key, out value) ? value : @default;
		}

		public static TValue ItemOrNull<TKey, TValue>(this IDictionary<TKey, TValue> target, TKey key) where TValue : class
		{
			TValue value;
			return target.TryGetValue(key, out value) ? value : (TValue)null;
		}

		public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> target, TKey key, Func<TValue> creator, Action<TKey, TValue> changer)
		{
			if (!target.ContainsKey(key))
				target.ItemOrNew(key, creator);
			else
				changer(key, target[key]);
		}

		public static TValue ItemOrNew<TKey, TValue>(this IDictionary<TKey, TValue> target, TKey key, Func<TValue> creator)
		{
			TValue value;
			if (target.TryGetValue(key, out value))
				return value;

			value = creator();
			target.Add(key, value);
			return value;
		}

		public static TValue ItemOrNullIgnoreKeyCase<TValue>(this IDictionary<string, TValue> target, string key) where TValue : class
		{
			key = target.Keys.FirstOrDefault(x => string.Equals(key, x, StringComparison.OrdinalIgnoreCase));

			return key == null ? null : target[key];
		}
	}
}
