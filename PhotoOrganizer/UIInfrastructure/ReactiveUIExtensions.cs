using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace PhotoOrganizer.UIInfrastructure
{
    public static class ReactiveUIExtensions
    {
        public static void WhenActivated(this ISupportsActivation source, Action action)
        {
            source.WhenActivated(compositeDisposable => WhenActivatedDummy(compositeDisposable, action));
        }

        // This has to be done since without providing a concrete type, the compiler doesn't know which extension method of WhenActivated to use.
        private static void WhenActivatedDummy(CompositeDisposable compositeDisposable, Action action) => action();
    }
}