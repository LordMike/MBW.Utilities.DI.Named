using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

public class DisposalTest
{
    /// <summary>
    /// Tests disposal in regular MS DI, to confirm behaviour
    /// </summary>
    [Fact]
    public void TestRegularDisposal()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddScoped(_ => new InternalClass())
            .BuildServiceProvider(true);

        InternalClass instance;
        using (IServiceScope scope = provider.CreateScope())
        {
            instance = scope.ServiceProvider.GetRequiredService<InternalClass>();
            Assert.False(instance.IsDisposed);
        }

        Assert.True(instance.IsDisposed);
    }

    [Fact]
    public void TestScopedDisposal()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddScoped("MyName", _ => new InternalClass())
            .BuildServiceProvider(true);

        InternalClass instance;
        using (IServiceScope scope = provider.CreateScope())
        {
            instance = scope.ServiceProvider.GetRequiredService<InternalClass>("MyName");
            Assert.False(instance.IsDisposed);
        }

        Assert.True(instance.IsDisposed);

        // Twice
        using (IServiceScope scope = provider.CreateScope())
        {
            instance = scope.ServiceProvider.GetRequiredService<InternalClass>("MyName");
            Assert.False(instance.IsDisposed);
        }

        Assert.True(instance.IsDisposed);
    }

    [Fact]
    public void TestRootSingletonDisposal()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddSingleton("MyName", _ => new InternalClass())
            .BuildServiceProvider(true);

        InternalClass instance;
        using (provider)
        {
            instance = provider.GetRequiredService<InternalClass>("MyName");
            Assert.False(instance.IsDisposed);
        }

        Assert.True(instance.IsDisposed);
    }

    [Fact]
    public void TestRootTransientDisposal()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddTransient("MyName", _ => new InternalClass())
            .BuildServiceProvider(true);

        InternalClass instanceA;
        InternalClass instanceB;
        using (provider)
        {
            instanceA = provider.GetRequiredService<InternalClass>("MyName");
            instanceB = provider.GetRequiredService<InternalClass>("MyName");

            Assert.False(instanceA.IsDisposed);
            Assert.False(instanceB.IsDisposed);
        }

        Assert.True(instanceA.IsDisposed);
        Assert.True(instanceB.IsDisposed);
    }

    class InternalClass : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}