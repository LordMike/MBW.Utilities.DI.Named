using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

public class ScopedTests
{
    [Fact]
    public void Scoped()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddScoped("MyName", _ => new InternalClass())
            .BuildServiceProvider(true);

        Assert.Null(provider.GetService<InternalClass>());
        Assert.Null(provider.GetService<InternalClass>("OtherName"));
        Assert.Throws<InvalidOperationException>(() => provider.GetService<InternalClass>("MyName"));

        IServiceScope scope1 = provider.CreateScope();
        InternalClass scope1service1 = scope1.ServiceProvider.GetService<InternalClass>("MyName");
        InternalClass scope1service2 = scope1.ServiceProvider.GetService<InternalClass>("MyName");

        IServiceScope scope2 = provider.CreateScope();
        InternalClass scope2service1 = scope2.ServiceProvider.GetService<InternalClass>("MyName");
        InternalClass scope2service2 = scope2.ServiceProvider.GetService<InternalClass>("MyName");
        InternalClass scope2service3 = scope2.ServiceProvider.GetService<InternalClass>("MyName");

        Assert.NotEqual(scope1service1, scope2service1);
        Assert.Same(scope1service1, scope1service2);
        Assert.Same(scope2service1, scope2service2);

        Assert.Equal(2, InternalClass.CreatedCount);
    }

    class InternalClass
    {
        public static int CreatedCount { get; private set; }

        public InternalClass()
        {
            CreatedCount++;
        }
    }
}