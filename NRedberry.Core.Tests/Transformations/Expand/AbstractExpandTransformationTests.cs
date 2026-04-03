using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class AbstractExpandTransformationTests
{
    [Fact]
    public void ShouldUseDefaultTraverseGuideToSkipFunctions()
    {
        TestExpandTransformation transformation = new();
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("Sin[a*b]");

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        actual.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldUseProvidedTraverseGuideToEnterFunctions()
    {
        TestExpandTransformation transformation = new(TraverseGuide.All);

        NRedberry.Tensors.Tensor actual = transformation.Transform(TensorApi.Parse("Sin[a*b]"));

        actual.ToString(OutputFormat.Redberry).ShouldBe("Sin[p]");
    }

    [Fact]
    public void ShouldLeaveReciprocalPowerUntouchedInBaseImplementation()
    {
        TestExpandTransformation transformation = new();

        NRedberry.Tensors.Tensor actual = transformation.Transform(TensorApi.Parse("(a+b)**(-1)"));

        actual.ToString(OutputFormat.Redberry).ShouldBeOneOf("(a+b)**(-1)", "(b+a)**(-1)");
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
