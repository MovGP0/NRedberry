using System.Diagnostics.Contracts;
using Xunit;

namespace NRedberry.Core.Tests;

public sealed class GlobalsTests
{
    [Fact]
    public void ShouldExposeContractType()
    {
        Contract.EndContractBlock();
        Assert.NotNull(typeof(Contract));
    }
}
