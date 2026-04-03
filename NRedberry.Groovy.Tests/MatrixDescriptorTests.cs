using NRedberry.Transformations.Symmetrization;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Groovy.Tests;

public sealed class MatrixDescriptorTests
{
    [Fact]
    public void ShouldExposeRecordValueSemantics()
    {
        MatrixDescriptor descriptor = new(IndexType.Matrix1, 2, 3);

        descriptor.Type.ShouldBe(IndexType.Matrix1);
        descriptor.Upper.ShouldBe(2);
        descriptor.Lower.ShouldBe(3);
        descriptor.ShouldBe(new MatrixDescriptor(IndexType.Matrix1, 2, 3));
    }
}

public sealed class TestTransformation : TransformationToStringAble
{
    public Tensor Transform(Tensor t)
    {
        return t;
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "test-transformation";
    }

    public override string ToString()
    {
        return "test-transformation";
    }
}
