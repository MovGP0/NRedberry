using NRedberry.Core.Tensors;
using NRedberry.Core.Tensors.Functions;
using NRedberry.Core.Transformations.Substitutions;

namespace NRedberry.Core.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.DifferentiateTransformation.
/// </summary>
public sealed class DifferentiateTransformation : TransformationToStringAble
{
    private readonly SimpleTensor[] vars;
    private readonly ITransformation[] expandAndContract;
    private readonly bool useDeltaFunction;

    public DifferentiateTransformation(params SimpleTensor[] vars)
    {
        throw new NotImplementedException();
    }

    public DifferentiateTransformation(bool useDeltaFunction, params SimpleTensor[] vars)
    {
        throw new NotImplementedException();
    }

    public DifferentiateTransformation(SimpleTensor[] vars, ITransformation[] expandAndContract)
    {
        throw new NotImplementedException();
    }

    public DifferentiateTransformation(bool useDeltaFunction, SimpleTensor[] vars, ITransformation[] expandAndContract)
    {
        throw new NotImplementedException();
    }

    public DifferentiateTransformation(SimpleTensor[] vars, DifferentiateOptions options)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public static Tensor Differentiate(Tensor tensor, SimpleTensor var, int order)
    {
        throw new NotImplementedException();
    }

    public static Tensor Differentiate(Tensor tensor, SimpleTensor var, int order, bool useDeltaFunction)
    {
        throw new NotImplementedException();
    }

    public static Tensor Differentiate(Tensor tensor, params SimpleTensor[] vars)
    {
        throw new NotImplementedException();
    }

    public static Tensor Differentiate(Tensor tensor, bool useDeltaFunction, params SimpleTensor[] vars)
    {
        throw new NotImplementedException();
    }

    public static Tensor Differentiate(Tensor tensor, ITransformation[] expandAndContract, bool useDeltaFunction, params SimpleTensor[] vars)
    {
        throw new NotImplementedException();
    }

    private static Tensor Differentiate(Tensor tensor, ITransformation[] expandAndContract, SimpleTensor var, bool useDeltaFunction)
    {
        throw new NotImplementedException();
    }

    private static bool ContainsIndicesNames(ISet<int> set, Indices.Indices indices)
    {
        throw new NotImplementedException();
    }

    private static bool ContainsIndicesNames(ISet<int> set, int[] indices)
    {
        throw new NotImplementedException();
    }

    private static Tensor DifferentiateWithRenaming(Tensor tensor, SimpleTensorDifferentiationRule rule, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    private static Tensor DifferentiateSimpleTensor(SimpleTensor tensor, SimpleTensorDifferentiationRule rule, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    private static Tensor DifferentiateInternal(Tensor tensor, SimpleTensorDifferentiationRule rule, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    private static Tensor ApplyTransformations(Tensor tensor, ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    private static SimpleTensorDifferentiationRule CreateRule(SimpleTensor var, bool useDeltaFunction)
    {
        throw new NotImplementedException();
    }

    private static Tensor[] CreateTensorFieldDerivativeArguments(SimpleTensor field, SimpleTensor var, SimpleTensor[] vars)
    {
        throw new NotImplementedException();
    }

    private static SubstitutionTransformation CreateDerivativeViaDelta(SimpleTensor[] vars, Tensor expression)
    {
        throw new NotImplementedException();
    }

    private static Tensor DifferentiateScalarFunction(ScalarFunction function, SimpleTensor[] vars)
    {
        throw new NotImplementedException();
    }

    private static Tensor Differentiate(Tensor tensor, ITransformation[] expandAndContract, bool useDeltaFunction, SimpleTensor[] vars, int index)
    {
        throw new NotImplementedException();
    }

    private static Tensor DifferentiateWithDelta(Tensor tensor, SimpleTensor[] vars, bool useDeltaFunction)
    {
        throw new NotImplementedException();
    }

    private abstract class SimpleTensorDifferentiationRule
    {
        protected SimpleTensorDifferentiationRule(SimpleTensor var, bool useDeltaFunction)
        {
            throw new NotImplementedException();
        }

        public virtual Tensor DifferentiateSimpleTensor(SimpleTensor simpleTensor)
        {
            throw new NotImplementedException();
        }

        public virtual SimpleTensorDifferentiationRule NewRuleForTensor(Tensor tensor)
        {
            throw new NotImplementedException();
        }

        public virtual Tensor DifferentiateSimpleTensorWithoutCheck(SimpleTensor simpleTensor)
        {
            throw new NotImplementedException();
        }

        public virtual int[] GetForbidden()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class SymbolicDifferentiationRule : SimpleTensorDifferentiationRule
    {
        public SymbolicDifferentiationRule(SimpleTensor var, bool useDeltaFunction)
            : base(var, useDeltaFunction)
        {
            throw new NotImplementedException();
        }

        public override Tensor DifferentiateSimpleTensorWithoutCheck(SimpleTensor simpleTensor)
        {
            throw new NotImplementedException();
        }

        public override SimpleTensorDifferentiationRule NewRuleForTensor(Tensor tensor)
        {
            throw new NotImplementedException();
        }

        public override int[] GetForbidden()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class SymmetricDifferentiationRule : SimpleTensorDifferentiationRule
    {
        public SymmetricDifferentiationRule(SimpleTensor var, bool useDeltaFunction)
            : base(var, useDeltaFunction)
        {
            throw new NotImplementedException();
        }

        private SymmetricDifferentiationRule(SimpleTensor var, Tensor derivative, int[] allFreeFrom, int[] freeVarIndices, bool useDeltaFunction)
            : base(var, useDeltaFunction)
        {
            throw new NotImplementedException();
        }

        public override Tensor DifferentiateSimpleTensorWithoutCheck(SimpleTensor simpleTensor)
        {
            throw new NotImplementedException();
        }

        public override SimpleTensorDifferentiationRule NewRuleForTensor(Tensor tensor)
        {
            throw new NotImplementedException();
        }

        public override int[] GetForbidden()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class DifferentiateOptions
    {
        public ITransformation Simplifications
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public bool UseDiracDelta
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public DifferentiateOptions()
        {
            throw new NotImplementedException();
        }

        public DifferentiateOptions(ITransformation simplifications)
        {
            throw new NotImplementedException();
        }
    }
}
