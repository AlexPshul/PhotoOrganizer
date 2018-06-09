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
    }
}