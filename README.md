## MBW.Utilities.DI.Named [![Generic Build](https://github.com/LordMike/MBW.Utilities.DI.Named/actions/workflows/dotnet.yml/badge.svg)](https://github.com/LordMike/MBW.Utilities.DI.Named/actions/workflows/dotnet.yml) [![NuGet](https://img.shields.io/nuget/v/MBW.Utilities.DI.Named.svg)](https://www.nuget.org/packages/MBW.Utilities.DI.Named) [![GHPackages](https://img.shields.io/badge/package-alpha-green)](https://github.com/LordMike/MBW.Utilities.DI.Named/packages/690356)

Named services implementation for `Microsoft.Extensions.DependencyInjection`

> [!TIP]
> In .NET 8.0, Microsoft introduced ["Keyed Services"](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-8.0#keyed-services) which may cover what you need. Do check that out before you use this library, as you may be able to achieve your task without additional assemblies.

## Features

You can add named services to a `ServiceCollection`, and later retrieve them. The library supports:

* Adding the same service under two different names
* Adding Transient, Scoped and Singleton services
* Disposals of services when the ServiceProvider is disposed
* Adding multiple services and retrieving them with `GetServices<T>(name)`
* Ability to retrieve all named services of `T` by `GetNamedServices<T>()`

## What you can't

You cannot inject named services into instances. The only way to use these named services, is by using the `IServiceProvider` and calling the appropriate extension methods on that.

### How to use

Add services as you normally would, and there'll generally be an overload of `Add*<T>()` that takes a name. Retrieve services using the similar appropriate overloads that also take a name.

Adding services:

* `AddSingleton<TService>(string name)`
* `AddSingleton<TService, TImplementation>(string name)`
* `AddSingleton<TService>(string name, TService instance)`
* `AddSingleton<TService>(string name, Func<IServiceProvider, TService> factory)`
* `AddScoped<TService>(string name)`
* `AddScoped<TService, TImplementation>(string name)`
* `AddScoped<TService>(string name, TService instance)`
* `AddScoped<TService>(string name, Func<IServiceProvider, TService> factory)`
* `AddTransient<TService>(string name)`
* `AddTransient<TService, TImplementation>(string name)`
* `AddTransient<TService>(string name, TService instance)`
* `AddTransient<TService>(string name, Func<IServiceProvider, TService> factory)`
* `TryAddSingleton<TService>(string name, TService instance)`
* `TryAddSingleton<TService>(string name, Func<IServiceProvider, TService> factory)`
* `TryAddSingleton<TService>(string name)`
* `TryAddSingleton<TService, TImplementation>(string name)`
* `TryAddScoped<TService>(string name, TService instance)`
* `TryAddScoped<TService>(string name, Func<IServiceProvider, TService> factory)`
* `TryAddScoped<TService>(string name)`
* `TryAddScoped<TService, TImplementation>(string name)`
* `TryAddTransient<TService>(string name, TService instance)`
* `TryAddTransient<TService>(string name, Func<IServiceProvider, TService> factory)`
* `TryAddTransient<TService>(string name)`
* `TryAddTransient<TService, TImplementation>(string name)`

Retrieving services from a `ServiceProvider`:

* `T GetService<T>(this IServiceProvider provider, string name)`
* `IEnumerable<(string name, T service)> GetNamedServices<T>(this IServiceProvider provider)`
* `T GetRequiredService<T>(this IServiceProvider provider, string name)`
* `IEnumerable<T> GetServices<T>(this IServiceProvider provider, string name)`
