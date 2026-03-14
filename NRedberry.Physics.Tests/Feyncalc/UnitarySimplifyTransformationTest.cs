using NRedberry.Physics.Feyncalc;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;
using Xunit.Abstractions;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class UnitarySimplifyTransformationTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void ShouldThrowUntilUnitarySimplifyTransformationIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new UnitarySimplifyTransformation(
            TensorFactory.ParseSimple("T_A"),
            TensorFactory.ParseSimple("f_ABC"),
            TensorFactory.ParseSimple("d_ABC"),
            TensorFactory.Parse("N")));
    }

    [Fact]
    public void Test1()
    {
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.
        // TODO: setAntiSymmetric/setSymmetric are not yet ported.

        UnitarySimplifyTransformation tr = new(
            TensorFactory.ParseSimple("T_A"),
            TensorFactory.ParseSimple("f_ABC"),
            TensorFactory.ParseSimple("d_ABC"),
            TensorFactory.ParseSimple("n"));

        testOutputHelper.WriteLine(tr.Transform(TensorFactory.Parse("T_A*T^A")).ToString());
    }
}
