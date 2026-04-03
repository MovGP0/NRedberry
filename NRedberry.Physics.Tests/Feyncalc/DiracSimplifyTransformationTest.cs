using NRedberry.Numbers;
using NRedberry.Physics.Feyncalc;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class DiracSimplifyTransformationTest
{
    [Fact]
    public void ShouldConstructAndLeaveScalarUntouched()
    {
        DiracSimplifyTransformation transformation = new(new DiracOptions());

        transformation.ToString(OutputFormat.Redberry).ShouldBe("DiracSimplify");
        ReferenceEquals(Complex.One, transformation.Transform(Complex.One)).ShouldBeTrue();
    }
}
