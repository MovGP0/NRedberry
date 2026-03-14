using System.Diagnostics.Contracts;
using Shouldly;
using Xunit;

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
