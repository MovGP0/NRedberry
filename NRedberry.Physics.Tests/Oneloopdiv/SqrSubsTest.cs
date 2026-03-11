using System;
using System.Reflection;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Oneloopdiv;

public sealed class SqrSubsTest
{
    [Fact]
    public void ShouldEliminateSquareNormInsideProduct()
    {
        SimpleTensor n = (SimpleTensor)TensorFactory.Parse("n_{a}");
        ITransformation tr = CreateSqrSubs(n);
        Tensor t = TensorFactory.Parse("n_m*n^m*a*n_a*n^a*n_i*n^j*b");
        t = tr.Transform(t);
        string actual = t.ToString();
        Assert.Contains("a", actual);
        Assert.Contains("b", actual);
        Assert.Contains("n_{i}", actual);
        Assert.Contains("n^{j}", actual);
        Assert.DoesNotContain("n_{a}", actual);
        Assert.DoesNotContain("n^{a}", actual);
        Assert.DoesNotContain("n_{m}", actual);
        Assert.DoesNotContain("n^{m}", actual);
    }

    [Fact]
    public void ShouldEliminateSquareNormInsideSum()
    {
        SimpleTensor n = (SimpleTensor)TensorFactory.Parse("n_{a}");
        ITransformation tr = CreateSqrSubs(n);
        Tensor t = TensorFactory.Parse("n_m*n^m*n_a*n^a+2");
        Tensor transformed = TensorFactory.Sum(tr.Transform(t[0]), tr.Transform(t[1]));
        AssertEquals("3", transformed);
    }

    private static ITransformation CreateSqrSubs(SimpleTensor tensor)
    {
        const string typeName = "NRedberry.Physics.Oneloopdiv.SqrSubs, NRedberry.Physics";
        Type? sqrSubsType = Type.GetType(typeName);
        if (sqrSubsType is null)
        {
            throw new InvalidOperationException($"Unable to resolve {typeName}.");
        }

        object? instance = Activator.CreateInstance(
            sqrSubsType,
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            null,
            new object[] { tensor },
            null);

        if (instance is not ITransformation transformation)
        {
            throw new InvalidOperationException("SqrSubs did not implement ITransformation as expected.");
        }

        return transformation;
    }

    private static void AssertEquals(string expected, Tensor actual)
    {
        Assert.Equal(TensorFactory.Parse(expected).ToString(), actual.ToString());
    }
}
