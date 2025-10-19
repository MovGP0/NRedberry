﻿using NRedberry.Core.Tensors;
using NRedberry.Core.Tensors.Iterators;
using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandAllTransformation.
/// </summary>
public sealed class ExpandAllTransformation : AbstractExpandTransformation
{
    public static ExpandAllTransformation Instance => throw new NotImplementedException();

    private ExpandAllTransformation()
        : base()
    {
        throw new NotImplementedException();
    }

    public ExpandAllTransformation(ITransformation[] transformations)
        : base(transformations)
    {
        throw new NotImplementedException();
    }

    public ExpandAllTransformation(ITransformation[] transformations, TraverseGuide traverseGuide)
        : base(transformations, traverseGuide)
    {
        throw new NotImplementedException();
    }

    public ExpandAllTransformation(ExpandOptions options)
        : base(options)
    {
        throw new NotImplementedException();
    }

    public static Tensor Expand(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor Expand(Tensor tensor, params ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    protected override Tensor ExpandProduct(Product product, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public override string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
