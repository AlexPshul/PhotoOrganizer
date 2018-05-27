using System;
using Windows.UI.Xaml;
using ReactiveUI;

namespace PhotoOrganizer.UIInfrastructure
{
    internal class CustomViewLocator : IViewLocator
    {
        private const string ViewModelSuffix = "ViewModel";
        private const string ModelSuffix = "Model";

        private static readonly string FindViewErrorMessageTemplate = @"Can't locate view for {0}";

        public IViewFor ResolveView<T>(T viewModel, string contract = null) where T : class
        {
            Type viewModelType = viewModel.GetType();

            Type viewType = GetViewType(viewModelType);
            if (viewType == null)
                throw new Exception(string.Format(FindViewErrorMessageTemplate, typeof(T).Name));

            if (!(Activator.CreateInstance(viewType) is FrameworkElement element))
                throw new Exception(string.Format(FindViewErrorMessageTemplate, typeof(T).Name));

            element.DataContext = viewModel;
            return element as IViewFor;
        }

        private static Type GetViewType(Type viewModelType)
        {
            if (viewModelType.IsGenericType)
            {
                int genericSuffixIndex = viewModelType.Name.IndexOf('`');
                if (!viewModelType.Name.Remove(genericSuffixIndex).EndsWith(ViewModelSuffix))
                    return null;
            }
            else if (!viewModelType.Name.EndsWith(ViewModelSuffix))
                return null;

            return SameAssemblySearch(viewModelType);
        }

        private static Type SameAssemblySearch(Type viewModelType)
        {
            string viewModelFullName = viewModelType.IsGenericType ? RemoveGenericTypeNameSuffix(viewModelType.FullName) : viewModelType.FullName;
            string viewTypeName = viewModelFullName
                .Remove(viewModelFullName.Length - ModelSuffix.Length)
                .Replace(".ViewModels.", ".Views.");

            var viewModelAssemblyName = viewModelType.Assembly.FullName;

            var viewName = $"{viewTypeName}, {viewModelAssemblyName}";
            return Type.GetType(viewName);
        }

        private static string RemoveGenericTypeNameSuffix(string typeName)
        {
            int genericSuffixIndex = typeName.IndexOf('`');
            return typeName.Remove(genericSuffixIndex);
        }
    }
}