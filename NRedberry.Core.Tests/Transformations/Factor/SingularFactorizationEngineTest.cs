using NRedberry.Indices;
using NRedberry.Transformations.Factor;
using NRedberry.Transformations.Expand;
using Xunit;
using RandomTensorGenerator = NRedberry.Tensors.Random.RandomTensor;
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

    [Fact(Skip = "Requires an external Singular installation; ignored in the Java tests.")]
    public void ShouldFactorRandomTensor()
    {
        RandomTensorGenerator random = new();
        random.ClearNamespace();
        random.AddToNamespace(
            TensorFactory.Parse("x"),
            TensorFactory.Parse("a"),
            TensorFactory.Parse("b"),
            TensorFactory.Parse("t"));

        using SingularFactorizationEngine engine = new("Singular");
        TensorType tensor = random.NextProductTree(3, 6, 8, IndicesFactory.EmptyIndices);

        tensor = ExpandTransformation.Expand(tensor);
        TensorType factored = engine.Transform(tensor);

        Assert.NotNull(factored);
    }
}
