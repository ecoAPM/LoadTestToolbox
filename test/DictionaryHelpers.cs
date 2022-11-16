using System.Collections.Concurrent;

namespace LoadTestToolbox.Tests;

public static class DictionaryHelpers
{
	public static ConcurrentDictionary<K, V> AsConcurrent<K, V>(this IDictionary<K, V> dictionary) where K : notnull
		=> new(dictionary);
}
