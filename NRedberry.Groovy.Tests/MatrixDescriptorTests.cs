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

        Assert.Equal(IndexType.Matrix1, descriptor.Type);
        Assert.Equal(2, descriptor.Upper);
        Assert.Equal(3, descriptor.Lower);
        Assert.Equal(descriptor, new MatrixDescriptor(IndexType.Matrix1, 2, 3));
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
