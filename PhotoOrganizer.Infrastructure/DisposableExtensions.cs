using System;
using System.Reactive.Disposables;

namespace PhotoOrganizer.Infrastructure
{
    public static class DisposableExtensions
    {
        public static IDisposable Create(Action onCreate, Action onDispose)
        {
            onCreate();
            return Disposable.Create(onDispose);
        }

        public static void MergeToComposite(this IDisposable source, CompositeDisposable compositeDisposable) => compositeDisposable.Add(source);
    }
}