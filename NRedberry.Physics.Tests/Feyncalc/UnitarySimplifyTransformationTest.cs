using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class UnitarySimplifyTransformationTest
{
    [Fact]
    public void ShouldPreserveBareGeneratorContractionWithoutInsertionRules()
    {
        UnitarySimplifyTransformation tr = new(
            TensorFactory.ParseSimple("T_A"),
            TensorFactory.ParseSimple("f_ABC"),
            TensorFactory.ParseSimple("d_ABC"),
            TensorFactory.ParseSimple("n"));

        Tensor input = TensorFactory.Parse("T_A*T^A");
        Tensor actual = tr.Transform(input);

        tr.ToString().ShouldBe("UnitarySimplify");
        actual.ToString().ShouldBe(input.ToString());
    }
}
