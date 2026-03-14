using NRedberry.Contexts;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class TransformationToStringAbleTests
{
    [Fact]
    public void ShouldComposeTransformationAndOutputFormattingContracts()
    {
        RecordingTransformation transformation = new();
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");
        IOutputFormattable outputFormattable = transformation;

        transformation.ShouldBeAssignableTo<ITransformation>();
        transformation.ShouldBeAssignableTo<IOutputFormattable>();
        transformation.Transform(tensor).ShouldBeSameAs(tensor);
        outputFormattable.ToString(OutputFormat.Redberry).ShouldBe("Redberry");
    }

    private sealed class RecordingTransformation : TransformationToStringAble
    {
        public NRedberry.Tensors.Tensor Transform(NRedberry.Tensors.Tensor tensor)
        {
            return tensor;
        }

        string IOutputFormattable.ToString(OutputFormat outputFormat)
        {
            return outputFormat.ToString();
        }
    }
}
