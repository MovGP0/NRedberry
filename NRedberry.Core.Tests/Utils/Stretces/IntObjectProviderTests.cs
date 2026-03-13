using NRedberry.Core.Utils.Stretces;
using Xunit;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class IntObjectProviderTests
{
    [Fact]
    public void ShouldUseHashProviderForObjectsAndNull()
    {
        IIntObjectProvider provider = IIntObjectProvider.HashProvider;

        Assert.Equal(17, provider.Get(new HashCarrier(17)));
        Assert.Equal(0, provider.Get(null!));
    }
}

internal sealed class HashCarrier(int hashCode)
{
    public override int GetHashCode()
    {
        return hashCode;
    }
}
