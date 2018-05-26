using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Autofac;
using Splat;

namespace PhotoOrganizer.Infrastructure
{
    public class AutofacMutableDependencyResolver : IMutableDependencyResolver
    {
        #region Private Members

        private readonly ContainerBuilder _builder = new ContainerBuilder();
        private IContainer _container;

        private readonly Dictionary<(Type, string), Func<object>> _beforeBuildRegistrations = new Dictionary<(Type, string), Func<object>>();

        #endregion

        #region Public Methods

        public void Build()
        {
            _builder.RegisterModule<Module>();
            _container = _builder.Build();
        }

        public void Dispose() => _container.Dispose();

        public object GetService(Type serviceType, string contract = null)
        {
            if (_container == null)
                return _beforeBuildRegistrations[(serviceType, contract)]();

            return string.IsNullOrEmpty(contract)
                ? _container.Resolve(serviceType)
                : _container.ResolveKeyed(contract, serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            var enumerableOfServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return (IEnumerable<object>)GetService(enumerableOfServiceType, contract);
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            if (_container != null)
                throw new Exception("Adding new registrations to the module after initial build up is not supported.");

            _builder.Register(_ => factory()).As(serviceType);
            _beforeBuildRegistrations[(serviceType, contract)] = factory;
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback) => Disposable.Empty;

        #endregion
    }
}