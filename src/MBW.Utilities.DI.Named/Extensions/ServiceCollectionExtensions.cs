using System;
using MBW.Utilities.DI.Named.Implementation;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace 
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection AddNamed(IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
    {
        Type registrationType = RegistrationTypeManager.GetRegistrationWrapperType(serviceType, name, true);

        services
            .Add(new ServiceDescriptor(registrationType, ctx =>
            {
                RegistrationWrapper wrapperInstance = (RegistrationWrapper)Activator.CreateInstance(registrationType);
                wrapperInstance.Factory = factory;

                return wrapperInstance;
            }, lifetime));

        return services;
    }

    private static IServiceCollection TryAddNamed(IServiceCollection services, string name, Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime)
    {
        Type registrationType = RegistrationTypeManager.GetRegistrationWrapperType(serviceType, name, true);

        services
            .TryAdd(new ServiceDescriptor(registrationType, ctx =>
            {
                RegistrationWrapper wrapperInstance = (RegistrationWrapper)Activator.CreateInstance(registrationType);
                wrapperInstance.Factory = factory;

                return wrapperInstance;
            }, lifetime));

        return services;
    }

    private static IServiceCollection AddNamed(IServiceCollection services, string name, Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        return AddNamed(services, name, serviceType, provider => ActivatorUtilities.CreateInstance(provider, implementationType), lifetime);
    }

    private static IServiceCollection TryAddNamed(IServiceCollection services, string name, Type serviceType, Type implementationType, ServiceLifetime lifetime)
    {
        return TryAddNamed(services, name, serviceType, provider => ActivatorUtilities.CreateInstance(provider, implementationType), lifetime);
    }

    #region Generic add

    public static IServiceCollection Add<TService>(this IServiceCollection services, string name, ServiceLifetime lifetime)
    {
        return Add<TService, TService>(services, name, lifetime);
    }

    public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, string name, ServiceLifetime lifetime) where TImplementation : TService
    {
        return AddNamed(services, name, typeof(TService), typeof(TImplementation), lifetime);
    }

    public static IServiceCollection Add<TService>(this IServiceCollection services, string name, ServiceLifetime lifetime, TService instance)
    {
        return AddNamed(services, name, typeof(TService), _ => instance, lifetime);
    }

    public static IServiceCollection Add<TService>(this IServiceCollection services, string name, ServiceLifetime lifetime, Func<IServiceProvider, TService> factory)
    {
        return AddNamed(services, name, typeof(TService), provider => factory(provider), lifetime);
    }

    public static IServiceCollection TryAdd<TService>(this IServiceCollection services, string name, ServiceLifetime lifetime)
    {
        return TryAdd<TService, TService>(services, name, lifetime);
    }

    public static IServiceCollection TryAdd<TService, TImplementation>(this IServiceCollection services, string name, ServiceLifetime lifetime) where TImplementation : TService
    {
        return TryAddNamed(services, name, typeof(TService), typeof(TImplementation), lifetime);
    }

    public static IServiceCollection TryAdd<TService>(this IServiceCollection services, string name, ServiceLifetime lifetime, TService instance)
    {
        return TryAddNamed(services, name, typeof(TService), _ => instance, lifetime);
    }

    public static IServiceCollection TryAdd<TService>(this IServiceCollection services, string name, ServiceLifetime lifetime, Func<IServiceProvider, TService> factory)
    {
        return TryAddNamed(services, name, typeof(TService), provider => factory(provider), lifetime);
    }

    #endregion

    #region Singleton

    public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, string name)
    {
        return AddNamed(services, name, typeof(TService), typeof(TService), ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, string name) where TImplementation : TService
    {
        return AddNamed(services, name, typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, string name, TService instance)
    {
        return AddNamed(services, name, typeof(TService), _ => instance, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> factory)
    {
        return AddNamed(services, name, typeof(TService), provider => factory(provider), ServiceLifetime.Singleton);
    }

    public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection services, string name)
    {
        return TryAddNamed(services, name, typeof(TService), typeof(TService), ServiceLifetime.Singleton);
    }

    public static IServiceCollection TryAddSingleton<TService, TImplementation>(this IServiceCollection services, string name) where TImplementation : TService
    {
        return TryAddNamed(services, name, typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
    }

    public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection services, string name, TService instance)
    {
        return TryAddNamed(services, name, typeof(TService), _ => instance, ServiceLifetime.Singleton);
    }

    public static IServiceCollection TryAddSingleton<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> factory)
    {
        return TryAddNamed(services, name, typeof(TService), provider => factory(provider), ServiceLifetime.Singleton);
    }

    #endregion

    #region Scoped

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, string name)
    {
        return AddNamed(services, name, typeof(TService), typeof(TService), ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, string name) where TImplementation : TService
    {
        return AddNamed(services, name, typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, string name, TService instance)
    {
        return AddNamed(services, name, typeof(TService), _ => instance, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddScoped<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> factory)
    {
        return AddNamed(services, name, typeof(TService), provider => factory(provider), ServiceLifetime.Scoped);
    }

    public static IServiceCollection TryAddScoped<TService>(this IServiceCollection services, string name)
    {
        return TryAddNamed(services, name, typeof(TService), typeof(TService), ServiceLifetime.Scoped);
    }

    public static IServiceCollection TryAddScoped<TService, TImplementation>(this IServiceCollection services, string name) where TImplementation : TService
    {
        return TryAddNamed(services, name, typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);
    }

    public static IServiceCollection TryAddScoped<TService>(this IServiceCollection services, string name, TService instance)
    {
        return TryAddNamed(services, name, typeof(TService), _ => instance, ServiceLifetime.Scoped);
    }

    public static IServiceCollection TryAddScoped<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> factory)
    {
        return TryAddNamed(services, name, typeof(TService), provider => factory(provider), ServiceLifetime.Scoped);
    }

    #endregion

    #region Transient

    public static IServiceCollection AddTransient<TService>(this IServiceCollection services, string name)
    {
        return AddNamed(services, name, typeof(TService), typeof(TService), ServiceLifetime.Transient);
    }

    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, string name) where TImplementation : TService
    {
        return AddNamed(services, name, typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
    }

    public static IServiceCollection AddTransient<TService>(this IServiceCollection services, string name, TService instance)
    {
        return AddNamed(services, name, typeof(TService), _ => instance, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddTransient<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> factory)
    {
        return AddNamed(services, name, typeof(TService), provider => factory(provider), ServiceLifetime.Transient);
    }

    public static IServiceCollection TryAddTransient<TService>(this IServiceCollection services, string name)
    {
        return TryAddNamed(services, name, typeof(TService), typeof(TService), ServiceLifetime.Transient);
    }

    public static IServiceCollection TryAddTransient<TService, TImplementation>(this IServiceCollection services, string name) where TImplementation : TService
    {
        return TryAddNamed(services, name, typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
    }

    public static IServiceCollection TryAddTransient<TService>(this IServiceCollection services, string name, TService instance)
    {
        return TryAddNamed(services, name, typeof(TService), _ => instance, ServiceLifetime.Transient);
    }

    public static IServiceCollection TryAddTransient<TService>(this IServiceCollection services, string name, Func<IServiceProvider, TService> factory)
    {
        return TryAddNamed(services, name, typeof(TService), provider => factory(provider), ServiceLifetime.Transient);
    }

    #endregion
    
    #region Management

    public static IServiceCollection RemoveAll<T>(this IServiceCollection services, string name)
    {
        return services.RemoveAll(typeof(T), name);
    }

    public static IServiceCollection RemoveAll(this IServiceCollection services, Type serviceType, string name)
    {
        Type registrationType = RegistrationTypeManager.GetRegistrationWrapperType(serviceType, name, false);
        if (registrationType == null)
            return services;

        return services.RemoveAll(registrationType);
    }

    #endregion
}