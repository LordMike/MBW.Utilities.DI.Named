using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

public class MultipleServicesTests
{
    [Fact]
    public void TransientServices()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddTransient<ITypeBase>("MyName", _ => new TypeA())
            .AddTransient<ITypeBase>("MyName", _ => new TypeB())
            .BuildServiceProvider(true);

        List<ITypeBase> services = provider.GetServices<ITypeBase>("MyName").ToList();

        Assert.Equal(2, services.Count);
        Assert.Contains(services, element => element is TypeA);
        Assert.Contains(services, element => element is TypeB);
    }

    [Fact]
    public void SingletonServices()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddSingleton<ITypeBase>("MyName", _ => new TypeA())
            .AddSingleton<ITypeBase>("MyName", _ => new TypeB())
            .BuildServiceProvider(true);

        List<ITypeBase> services = provider.GetServices<ITypeBase>("MyName").ToList();

        Assert.Equal(2, services.Count);
        Assert.Contains(services, element => element is TypeA);
        Assert.Contains(services, element => element is TypeB);
    }

    [Fact]
    public void ScopedServices()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddScoped<ITypeBase>("MyName", _ => new TypeA())
            .AddScoped<ITypeBase>("MyName", _ => new TypeB())
            .BuildServiceProvider(true);

        using (IServiceScope scope = provider.CreateScope())
        {
            List<ITypeBase> services = scope.ServiceProvider.GetServices<ITypeBase>("MyName").ToList();

            Assert.Equal(2, services.Count);
            Assert.Contains(services, element => element is TypeA);
            Assert.Contains(services, element => element is TypeB);
        }
    }

    [Fact]
    public void GetAllNamedServices_CrossDIContamination()
    {
        _ = new ServiceCollection()
            .AddSingleton<ITypeBase>("first", _ => new TypeA())
            .BuildServiceProvider(true);

        ServiceProvider provider = new ServiceCollection()
            .AddSingleton<ITypeBase>("second", _ => new TypeA())
            .BuildServiceProvider(true);

        List<(string name, ITypeBase service)> services = provider.GetNamedServices<ITypeBase>().ToList();

        Assert.Single(services);
        Assert.Contains(services, element => element.name == "second" && element.service is TypeA);
    }

    [Fact]
    public void GetAllNamedServices()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddSingleton<ITypeBase>("MyFirstName", _ => new TypeA())
            .AddSingleton<ITypeBase>("MyOtherName", _ => new TypeB())
            .BuildServiceProvider(true);

        List<(string name, ITypeBase service)> services = provider.GetNamedServices<ITypeBase>().ToList();

        Assert.Equal(2, services.Count);
        Assert.Contains(services, element => element.name == "MyFirstName" && element.service is TypeA);
        Assert.Contains(services, element => element.name == "MyOtherName" && element.service is TypeB);
    }

    [Fact]
    public void TryAddServices()
    {
        ServiceProvider provider = new ServiceCollection()
            .TryAddTransient<ITypeBase>("MyName", _ => new TypeA())
            .TryAddTransient<ITypeBase>("MyName", _ => new TypeB())
            .BuildServiceProvider(true);

        List<ITypeBase> lst = provider.GetServices<ITypeBase>("MyName").ToList();
        Assert.Single(lst);

        ITypeBase svc = provider.GetService<ITypeBase>("MyName");
        Assert.IsType<TypeA>(svc);
    }

    interface ITypeBase
    {
    }

    class TypeA : ITypeBase
    {
    }

    class TypeB : ITypeBase
    {
    }
}