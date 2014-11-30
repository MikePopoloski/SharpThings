using System;
using System.Collections.Generic;

namespace SharpThings {
    /// <summary>
    /// An implementation of the IServiceProvider interface for dependency injection.
    /// </summary>
    public sealed class ServiceProvider : IServiceProvider {
        Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service with the provider.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="service">The service to register.</param>
        public void Register<T>(T service) => services.Add(typeof(T), service);

        /// <summary>
        /// Requests a service from the provider.
        /// </summary>
        /// <typeparam name="T">The type of the service requested.</typeparam>
        /// <returns>The requested service, or default(T) if none is registered.</returns>
        public T Request<T>() => (T)services.Get(typeof(T));

        object IServiceProvider.GetService (Type serviceType) => services.Get(serviceType);
    }
}
