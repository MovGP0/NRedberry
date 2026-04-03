using NRedberry.Core.Utils.Stretces;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class IntObjectProviderTests
{
    [Fact]
    public void ShouldUseHashProviderForObjectsAndNull()
    {
        IIntObjectProvider provider = IIntObjectProvider.HashProvider;

        provider.Get(new HashCarrier(17)).ShouldBe(17);
        provider.Get(null!).ShouldBe(0);
    }
}

internal sealed class HashCarrier(int hashCode)
{
    public override int GetHashCode()
    {
        return hashCode;
    }
}
