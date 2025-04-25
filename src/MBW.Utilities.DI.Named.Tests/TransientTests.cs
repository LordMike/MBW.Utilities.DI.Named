using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

public class TransientTests
{
    [Fact]
    public void Transient()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddTransient("MyName", _ => new InternalClass())
            .BuildServiceProvider(true);

        Assert.Null(provider.GetService<InternalClass>());
        Assert.Null(provider.GetService<InternalClass>("OtherName"));

        InternalClass service1 = provider.GetService<InternalClass>("MyName");
        Assert.NotNull(service1);

        IServiceScope scope1 = provider.CreateScope();
        InternalClass scope1service1 = scope1.ServiceProvider.GetService<InternalClass>("MyName");
        InternalClass scope1service2 = scope1.ServiceProvider.GetService<InternalClass>("MyName");

        IServiceScope scope2 = provider.CreateScope();
        InternalClass scope2service1 = scope2.ServiceProvider.GetService<InternalClass>("MyName");
        InternalClass scope2service2 = scope2.ServiceProvider.GetService<InternalClass>("MyName");
        InternalClass scope2service3 = scope2.ServiceProvider.GetService<InternalClass>("MyName");

        Assert.NotSame(service1, scope1service1);
        Assert.NotSame(scope1service1, scope1service2);
        Assert.NotSame(scope1service2, scope2service1);
        Assert.NotSame(scope2service1, scope2service2);
        Assert.NotSame(scope2service2, scope2service3);

        Assert.Equal(6, InternalClass.CreatedCount);
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