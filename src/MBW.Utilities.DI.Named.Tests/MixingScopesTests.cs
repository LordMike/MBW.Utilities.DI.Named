using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MBW.Utilities.DI.Named.Tests;

/// <remarks>
/// By testing, it was not possible in Microsofts Service Provider to mix Scoped and non-scoped services - at least not in a way to get the Singleton/Transient service from the rootprovider.
/// </remarks>>
public class MixingScopesTests
{
    [Fact]
    public void SingletonAndTransientInSameCollection()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddSingleton<ICommonInterface, TypeA>("MyName")
            .AddTransient<ICommonInterface, TypeB>("MyName")
            .BuildServiceProvider(true);

        ICommonInterface lastAdded = provider.GetService<ICommonInterface>("MyName");
        Assert.IsType<TypeB>(lastAdded);

        List<ICommonInterface> set1 = provider.GetServices<ICommonInterface>("MyName").ToList();
        List<ICommonInterface> set2 = provider.GetServices<ICommonInterface>("MyName").ToList();

        Assert.Equal(2, set1.Count);
        Assert.Equal(2, set2.Count);

        List<ICommonInterface> combined = set1.Concat(set2).Distinct().ToList();
        Assert.Equal(3, combined.Count);

        Assert.Single(combined.OfType<TypeA>());
        Assert.Equal(2, combined.OfType<TypeB>().Count());
    }

    interface ICommonInterface
    {
    }

    class TypeA : ICommonInterface
    {
    }

    class TypeB : ICommonInterface
    {
    }
}