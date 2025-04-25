using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

public class RemovalTests
{
    [Fact]
    public void TestRemovalOfNamedService()
    {
        var services = new ServiceCollection();
        services.AddSingleton<object>("a");
        services.AddSingleton<object>("b");
        services.RemoveAll<object>("a");
        Assert.Single(services);
    }
}