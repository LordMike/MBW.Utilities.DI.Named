using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

public class SingletonTests
{
    [Fact]
    public void Singletons()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddSingleton("MyName", _ => new InternalClass())
            .BuildServiceProvider();

        Assert.Null(provider.GetService<InternalClass>());
        Assert.Null(provider.GetService<InternalClass>("OtherName"));

        InternalClass service1 = provider.GetService<InternalClass>("MyName");
        Assert.NotNull(service1);
        Assert.Equal(1, InternalClass.CreatedCount);

        InternalClass service2 = provider.GetService<InternalClass>("MyName");
        Assert.NotNull(service2);
        Assert.Equal(1, InternalClass.CreatedCount);

        Assert.Same(service1, service2);
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