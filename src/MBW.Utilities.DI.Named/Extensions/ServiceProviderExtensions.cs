using System;
using System.Collections.Generic;
using System.Linq;
using MBW.Utilities.DI.Named.Implementation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static T GetService<T>(this IServiceProvider provider, string name)
    {
        Type registrationType = RegistrationTypeManager.GetRegistrationWrapperType(typeof(T), name, false);
        if (registrationType == null)
            return default;

        RegistrationWrapper wrapperService = (RegistrationWrapper)provider.GetService(registrationType);

        return wrapperService.GetInstance<T>(provider);
    }

    public static IEnumerable<(string name, T service)> GetNamedServices<T>(this IServiceProvider provider)
    {
        IEnumerable<(string name, Type registrationType)> registrationTypes =
            RegistrationTypeManager.GetRegistrationTypesAndNames(typeof(T));

        foreach ((string name, Type registrationType) registrationType in registrationTypes)
        {
            RegistrationWrapper wrapperService = (RegistrationWrapper)provider.GetService(registrationType.registrationType);
            if (wrapperService == null)
                continue;

            yield return (registrationType.name, wrapperService.GetInstance<T>(provider));
        }
    }

    public static T GetRequiredService<T>(this IServiceProvider provider, string name)
    {
        Type registrationType = RegistrationTypeManager.GetRegistrationWrapperType(typeof(T), name, false);
        if (registrationType == null)
            throw new Exception($"Service of type {typeof(T).FullName} not registered with name {name}");

        RegistrationWrapper wrapperService = (RegistrationWrapper)provider.GetRequiredService(registrationType);

        return wrapperService.GetInstance<T>(provider);
    }

    public static IEnumerable<T> GetServices<T>(this IServiceProvider provider, string name)
    {
        Type registrationType = RegistrationTypeManager.GetRegistrationWrapperType(typeof(T), name, false);
        if (registrationType == null)
            return Enumerable.Empty<T>();

        IEnumerable<RegistrationWrapper> wrapperServices =
            provider.GetServices(registrationType).Cast<RegistrationWrapper>();
        return wrapperServices.Select(s => s.GetInstance<T>(provider));
    }
}