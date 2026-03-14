using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

public abstract class AbstractFeynCalcTransformation : AbstractTransformationWithGammas
{
    private static IIndicator<GraphType> DefaultGraphFilter { get; } = new ExcludeGraphIndicator();

    protected AbstractFeynCalcTransformation(DiracOptions options, ITransformation? preprocessor)
        : base(options)
    {
        Preprocessor = preprocessor;
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        SubstitutionIterator iterator = new(tensor);
        while (iterator.Next() is Tensor original)
        {
            if (original is not Product product)
            {
                continue;
            }

            if (original.Indices.Size(MatrixType) == 0)
            {
                continue;
            }

            if (!ContainsGammaOr5Matrices(original))
            {
                continue;
            }

            Tensor current = (Preprocessor ?? Transformation.Identity).Transform(original);
            if (current is not Product processedProduct)
            {
                iterator.SafeSet(Transform(current));
                continue;
            }

            List<int> modifiedElements = [];
            List<Tensor> processed = [];

            ProductOfGammas.It gammas = new(GammaName, Gamma5Name, processedProduct, MatrixType, GraphFilter());
            while (gammas.Take() is ProductOfGammas line)
            {
                if (IsZeroTrace(line))
                {
                    iterator.Set(Complex.Zero);
                    goto ContinueOuter;
                }

                Tensor? transformedLine = TransformLine(line, modifiedElements);
                if (transformedLine is null)
                {
                    continue;
                }

                processed.Add(transformedLine);
                modifiedElements.AddRange(line.GPositions);
            }

            if (processed.Count == 0)
            {
                iterator.SafeSet(current);
                continue;
            }

            int offset = GetIndexlessOffset(processedProduct);
            for (int i = 0; i < modifiedElements.Count; ++i)
            {
                modifiedElements[i] += offset;
            }

            Tensor result = processedProduct.Remove(modifiedElements.ToArray());
            processed.Add(result);
            result = ExpandAndEliminate.Transform(
                TensorFactory.MultiplyAndRenameConflictingDummies(processed));
            result = TraceOfOne.Transform(result);
            result = DeltaTrace.Transform(result);
            iterator.SafeSet(result);

        ContinueOuter:
            continue;
        }

        return iterator.Result();
    }

    protected virtual IIndicator<GraphType> GraphFilter()
    {
        return DefaultGraphFilter;
    }

    protected bool IsZeroTrace(ProductOfGammas productOfGammas)
    {
        ArgumentNullException.ThrowIfNull(productOfGammas);

        return productOfGammas.GraphType == GraphType.Cycle
            && (((productOfGammas.Length - productOfGammas.G5Positions.Count) % 2) == 1
                || (productOfGammas.G5Positions.Count % 2 == 1
                    && (productOfGammas.Length - productOfGammas.G5Positions.Count) < 4));
    }

    protected virtual Tensor? TransformLine(ProductOfGammas productOfGammas, List<int> modifiedElements)
    {
        return null;
    }

    protected ITransformation? Preprocessor { get; }

    private bool ContainsGammaOr5Matrices(Tensor tensor)
    {
        if (tensor is SimpleTensor)
        {
            int hash = tensor.GetHashCode();
            return hash == GammaName || hash == Gamma5Name;
        }

        foreach (Tensor child in tensor)
        {
            if (ContainsGammaOr5Matrices(child))
            {
                return true;
            }
        }

        return false;
    }

    private static int GetIndexlessOffset(Product product)
    {
        return product.IndexlessData.Length + (product.Factor == Complex.One ? 0 : 1);
    }
}
