using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class AbstractExpandTransformationTests
{
    [Fact]
    public void ShouldUseDefaultTraverseGuideToSkipFunctions()
    {
        TestExpandTransformation transformation = new();
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("Sin[a*b]");

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.Same(tensor, actual);
    }

    [Fact]
    public void ShouldUseProvidedTraverseGuideToEnterFunctions()
    {
        TestExpandTransformation transformation = new(TraverseGuide.All);

        NRedberry.Tensors.Tensor actual = transformation.Transform(TensorApi.Parse("Sin[a*b]"));

        Assert.Equal("Sin[p]", actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldLeaveReciprocalPowerUntouchedInBaseImplementation()
    {
        TestExpandTransformation transformation = new();

        NRedberry.Tensors.Tensor actual = transformation.Transform(TensorApi.Parse("(a+b)**(-1)"));

        Assert.Equal("(a+b)**(-1)", actual.ToString(OutputFormat.Redberry));
    }

    private sealed class TestExpandTransformation : AbstractExpandTransformation
    {
        public TestExpandTransformation()
        {
        }

        public TestExpandTransformation(TraverseGuide traverseGuide)
            : base([], traverseGuide)
        {
        }

        protected override NRedberry.Tensors.Tensor ExpandProduct(
            NRedberry.Tensors.Product product,
            ITransformation[] transformations)
        {
            return TensorApi.Parse("p");
        }

        public override string ToString(OutputFormat outputFormat)
        {
            return nameof(TestExpandTransformation);
        }
    }
}
