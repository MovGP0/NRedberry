using System;
using System.Collections.Generic;
using NRedberry.Core.Concurrent;
using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandUtils.
/// </summary>
public static class ExpandUtils
{
    public sealed class ExpandPairPort : IOutputPort<Tensor>
    {
        public ExpandPairPort(Sum first, Sum second)
        {
            throw new NotImplementedException();
        }

        public ExpandPairPort(Sum first, Sum second, Tensor[] factors)
        {
            throw new NotImplementedException();
        }

        public Tensor Take()
        {
            throw new NotImplementedException();
        }
    }

    public static Tensor ExpandPairOfSums(Sum first, Sum second, Tensor[] factors, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandPairOfSums(Sum first, Sum second, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandProductOfSums(Product product, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public static Tensor Apply(ITransformation[] transformations, Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandProductOfSums(IEnumerable<Tensor> tensor, ITransformation[] transformations, bool indexed)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandProductOfSums1(IEnumerable<Tensor> tensor, ITransformation[] transformations, bool indexed)
    {
        throw new NotImplementedException();
    }

    public static Tensor MultiplySumElementsOnFactorAndExpand(Sum sum, Tensor factor, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public static Tensor MultiplySumElementsOnFactor(Sum sum, Tensor factor, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public static bool IsExpandablePower(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static bool SumContainsIndexed(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandSymbolicPower(Sum argument, int power, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandPower(Sum argument, int power, int[] forbiddenIndices, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public static ITransformation ExpandIndexlessSubproduct => throw new NotImplementedException();
}
