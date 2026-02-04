using System;
using System.Reflection;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Tests.Oneloopdiv;

public sealed class SqrSubsTest
{
    public void Test1()
    {
        SimpleTensor n = (SimpleTensor)TensorFactory.Parse("n_{a}");
        ITransformation tr = new Transformer(TraverseState.Leaving, new[] { CreateSqrSubs(n) });
        Tensor t = TensorFactory.Parse("n_m*n^m*a*n_a*n^a*n_i*n^j*b");
        t = tr.Transform(t);
        AssertEquals("a*b*n_{i}*n^{j}", t);
    }

    public void Test2()
    {
        SimpleTensor n = (SimpleTensor)TensorFactory.Parse("n_{a}");
        ITransformation tr = new Transformer(TraverseState.Leaving, new[] { CreateSqrSubs(n) });
        Tensor t = TensorFactory.Parse("n_m*n^m*n_a*n^a+2");
        t = tr.Transform(t);
        AssertEquals("3", t);
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
        if (!TensorUtils.Equals(TensorFactory.Parse(expected), actual))
        {
            throw new InvalidOperationException("Tensor comparison failed.");
        }
    }
}
