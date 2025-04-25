using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

public class ConstructorInjectionTest
{
    private const string ServiceName = "MyName";

    private void Validate<TServiceType>(IServiceProvider provider) where TServiceType : IInternalInterface
    {
        CheckType checkService = provider.GetRequiredService<CheckType>();

        IInternalInterface service = provider.GetService<TServiceType>(ServiceName);

        Assert.IsType<InternalClass>(service);
        Assert.NotNull(service.Provider);
        Assert.Same(checkService, service.Provider.GetRequiredService<CheckType>());
    }

    [Fact]
    public void AsSpecificType()
    {
        IServiceProvider provider = new ServiceCollection()
            .AddSingleton<InternalClass>(ServiceName)
            .AddSingleton(_ => new CheckType())
            .BuildServiceProvider(true);

        Validate<InternalClass>(provider);
    }

    [Fact]
    public void AsSpecificTypeWithFactory()
    {
        IServiceProvider provider = new ServiceCollection()
            .AddSingleton(ServiceName, prov => new InternalClass(prov))
            .AddSingleton(_ => new CheckType())
            .BuildServiceProvider(true);

        Validate<InternalClass>(provider);
    }

    [Fact]
    public void AsInterfaceWithImplementationType()
    {
        IServiceProvider provider = new ServiceCollection()
            .AddSingleton<IInternalInterface, InternalClass>(ServiceName)
            .AddSingleton(_ => new CheckType())
            .BuildServiceProvider(true);

        Validate<IInternalInterface>(provider);
    }

    [Fact]
    public void AsInterfaceWithFactory()
    {
        IServiceProvider provider = new ServiceCollection()
            .AddSingleton<IInternalInterface>(ServiceName, prov => new InternalClass(prov))
            .AddSingleton(_ => new CheckType())
            .BuildServiceProvider(true);

        Validate<IInternalInterface>(provider);
    }

    interface IInternalInterface
    {
        IServiceProvider Provider { get; }
    }

    class InternalClass : IInternalInterface
    {
        public IServiceProvider Provider { get; }

        public InternalClass(IServiceProvider provider)
        {
            Provider = provider;
        }
    }

    class CheckType
    {
    }
}