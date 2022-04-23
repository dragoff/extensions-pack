using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace ExtensionsPack
{
	public static class EnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
		{
			var i = 0;
			foreach (var element in enumerable)
			{
				action(element, i);
				++i;
			}
		}

		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var element in enumerable)
				action(element);
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable, System.Random random)
		{
			return enumerable.OrderBy(a => random.Next());
		}

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.OrderBy(a => Guid.NewGuid());
		}

		public static T RandomElement<T>(this IEnumerable<T> enumerable, System.Random random)
		{
			int count = enumerable.Count();
			return enumerable.ElementAt(random.Next(0, count));
		}

		public static T RandomElement<T>(this T[] array, System.Random random)
		{
			return array[random.Next(0, array.Length)];
		}

		public static T RandomElement<T>(this IEnumerable<T> enumerable)
		{
			int count = enumerable.Count();
			return enumerable.ElementAt(Random.Range(0, count));
		}

		public static T RandomElement<T>(this T[] array)
		{
			return array[Random.Range(0, array.Length)];
		}

		public static T RemoveRandom<T>(this IList<T> list)
		{
			if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
			int index = UnityEngine.Random.Range(0, list.Count);
			T item = list[index];
			list.RemoveAt(index);
			return item;
		}
		
		public static bool IsSubsetOf<T>(this IEnumerable<T> subset, IEnumerable<T> enumerable)
		{
			return !subset.Except(enumerable).Any();
		}

		public static bool IsContainsElementOf<T>(this IEnumerable<T> enumerable, IEnumerable<T> target)
		{
			return enumerable.Intersect(target).Any();
		}

		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
		{
			return new HashSet<T>(enumerable);
		}

		public static int IndexOf<T>(this IList<T> list, Func<T, bool> predicate)
		{
			for (var i = 0; i < list.Count; i++)
				if (predicate(list[i]))
					return i;

			return -1;
		}

		public static T Add<T>(this ICollection<T> list, T value)
		{
			list.Add(value);
			return value;
		}

		public static List<T> ToList<T>(this Array array)
		{
			var list = new List<T>(array.Length);
			foreach (var element in array)
				list.Add((T)element);

			return list;
		}

		public static TSource FirstMin<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector,
			IComparer<TKey> comparer = null)
		{
			return source.FirstMinMax(selector, comparer, true, false);
		}

		public static TSource FirstMax<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector,
			IComparer<TKey> comparer = null)
		{
			return source.FirstMinMax(selector, comparer, false, false);
		}

		public static TSource FirstMinOrDefault<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector,
			IComparer<TKey> comparer = null)
		{
			return source.FirstMinMax(selector, comparer, true, true);
		}

		public static TSource FirstMaxOrDefault<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector,
			IComparer<TKey> comparer = null)
		{
			return source.FirstMinMax(selector, comparer, false, true);
		}

		private static TSource FirstMinMax<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> selector,
			IComparer<TKey> comparer,
			bool seekMin, bool orDefault)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			comparer ??= Comparer<TKey>.Default;

			bool Compare(TKey candidateProjected, TKey key) => seekMin
				? comparer.Compare(candidateProjected, key) >= 0
				: comparer.Compare(candidateProjected, key) < 0;

			using var sourceIterator = source.GetEnumerator();

			if (!sourceIterator.MoveNext())
			{
				if (orDefault) return default;
				throw new InvalidOperationException("Sequence reached end or contains no elements");
			}

			var key = selector(sourceIterator.Current);

			return IterateWithCompare(selector, x => Compare(x, key), sourceIterator);
		}

		private static TSource IterateWithCompare<TSource, TKey>(
			Func<TSource, TKey> selector,
			Func<TKey, bool> comparer,
			IEnumerator<TSource> sourceIterator)
		{
			var current = sourceIterator.Current;

			while (sourceIterator.MoveNext())
			{
				var candidate = sourceIterator.Current;
				var candidateProjected = selector(candidate);
				if (comparer(candidateProjected))
					continue;

				current = candidate;
			}

			return current;
		}
	}
}