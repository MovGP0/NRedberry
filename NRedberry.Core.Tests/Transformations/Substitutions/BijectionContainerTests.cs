using NRedberry.IndexMapping;
using NRedberry.Transformations.Substitutions;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class BijectionContainerTests
{
    [Fact]
    public void ShouldStoreMappingAndBijection()
    {
        int[] bijection = [1, 2, 3];
        BijectionContainer container = new(Mapping.IdentityMapping, bijection);

        container.Mapping.ShouldBeSameAs(Mapping.IdentityMapping);
        container.Bijection.ShouldBeSameAs(bijection);
        container.ToString().ShouldContain("1");
    }
}
