using System;
using NRedberry.Tensors;
using NRedberry.Physics.Oneloopdiv;
using NRedberry.Numbers;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Oneloopdiv;

public sealed class SqrSubsTest
{
    [Fact]
    public void ShouldEliminateSquareNormInsideProduct()
    {
        SimpleTensor n = TensorFactory.ParseSimple("n_{a}");
        ITransformation tr = new SqrSubs(n);
        Tensor product = TensorFactory.Multiply(
            TensorFactory.ParseSimple("n_m"),
            TensorFactory.ParseSimple("n^m"),
            TensorFactory.ParseSimple("x"));

        Tensor transformed = tr.Transform(product);
        Assert.Equal("x", transformed.ToString());
    }

    [Fact]
    public void ShouldRejectTensorsWithMultipleIndices()
    {
        Assert.Throws<ArgumentException>(() => new SqrSubs(TensorFactory.ParseSimple("n_ab")));
    }
}
