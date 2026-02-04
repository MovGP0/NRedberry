using NRedberry.Transformations.Factor;
using Xunit;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class SingularFactorizationEngineTest
{
    [Fact(Skip = "Requires an external Singular installation; ignored in the Java tests.")]
    public void ShouldFactorSimplePolynomial()
    {
        using SingularFactorizationEngine engine = new("Singular");
        TensorType tensor = TensorFactory.Parse("12387623*x**134-12387623*y**6");

        _ = engine.Transform(tensor);
    }

    [Fact(Skip = "Requires Singular and RandomTensor (not yet ported).")]
    public void ShouldFactorRandomTensor()
    {
        // TODO: Port the RandomTensor-based scenario once RandomTensor is available.
    }
}
