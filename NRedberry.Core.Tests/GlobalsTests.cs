using System.Diagnostics.Contracts;

namespace NRedberry.Core.Tests;

public sealed class GlobalsTests
{
    [Fact]
    public void ShouldExposeContractType()
    {
        Contract.EndContractBlock();
        typeof(Contract).ShouldNotBeNull();
    }
}
