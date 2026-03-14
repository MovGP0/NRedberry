using System;
using NRedberry.IndexMapping;
using NRedberry.Transformations.Substitutions;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class BijectionContainerTests
{
    [Fact]
    public void ShouldStoreMappingAndBijection()
    {
        int[] bijection = [1, 2, 3];
        BijectionContainer container = new(Mapping.IdentityMapping, bijection);

        Assert.Same(Mapping.IdentityMapping, container.Mapping);
        Assert.Same(bijection, container.Bijection);
        Assert.Contains("1", container.ToString(), StringComparison.Ordinal);
    }
}
