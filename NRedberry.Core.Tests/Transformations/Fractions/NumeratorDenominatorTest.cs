using NRedberry.Tensors;
using NRedberry.Transformations.Fractions;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class NumeratorDenominatorTest
{
    [Fact]
    public void ShouldExtractNumeratorAndDenominator()
    {
        TensorType tensor = TensorFactory.Parse("b/(x+a)**(1/2)");
        NumeratorDenominator nd = NumeratorDenominator.GetNumeratorAndDenominator(tensor);

        TensorUtils.Equals(nd.Denominator, TensorFactory.Parse("(x+a)**(1/2)")).ShouldBeTrue();
        TensorUtils.Equals(nd.Numerator, TensorFactory.Parse("b")).ShouldBeTrue();
    }
}
