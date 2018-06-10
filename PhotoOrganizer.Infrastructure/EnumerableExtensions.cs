using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoOrganizer.Infrastructure
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source) => source.ToList().AsReadOnly();
        public static string JoinStrings(this IEnumerable<string> source, string separator) => string.Join(separator, source);
        public static Task<T[]> AwaitAll<T>(this IEnumerable<Task<T>> source) => Task.WhenAll(source);
        public static IEnumerable<T> Concat<T>(this T singleSource, IEnumerable<T> secondEnumerable) => new[] { singleSource }.Concat(secondEnumerable);
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T singleItem) => source.Concat(new[] { singleItem });
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) => source.SelectMany(_ => _);
    }
}