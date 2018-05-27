using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
    }
}