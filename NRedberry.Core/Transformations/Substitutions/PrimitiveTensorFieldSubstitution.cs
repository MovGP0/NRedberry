using System.Collections.Immutable;
using NRedberry.Contexts;
using NRedberry.IndexGeneration;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.PrimitiveTensorFieldSubstitution.
/// </summary>
public sealed class PrimitiveTensorFieldSubstitution : PrimitiveSubstitution
{
    private readonly NameDescriptorForTensorField _fromDescriptor;
    private readonly ImmutableArray<int> _orders;
    private readonly Dictionary<ImmutableArray<int>, DFromTo> _derivatives = [];

    public PrimitiveTensorFieldSubstitution(Tensor from, Tensor to)
        : base(from, to)
    {
        TensorField fromField = from as TensorField
            ?? throw new ArgumentException("Primitive tensor field substitution requires a tensor field source.", nameof(from));

        _fromDescriptor = fromField.GetNameDescriptor();
        _orders = [.. _fromDescriptor.GetDerivativeOrders()];
        _derivatives[_orders] = new DFromTo(fromField, to);
    }

    protected override Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator)
    {
        ArgumentNullException.ThrowIfNull(currentNode);
        ArgumentNullException.ThrowIfNull(iterator);

        TensorField currentField = (TensorField)currentNode;
        NameDescriptorForTensorField currentDescriptor = currentField.GetNameDescriptor();
        if (currentDescriptor.GetParent().Id != _fromDescriptor.GetParent().Id)
        {
            return currentNode;
        }

        for (int i = currentNode.Size - 1; i >= 0; --i)
        {
            if (currentDescriptor.GetDerivativeOrder(i) < _fromDescriptor.GetDerivativeOrder(i))
            {
                return currentNode;
            }
        }

        int[] derivativeOrders = currentDescriptor.GetDerivativeOrders();
        ImmutableArray<int> key = [.. derivativeOrders];
        if (!_derivatives.TryGetValue(key, out DFromTo? fromTo))
        {
            TensorField derivedFrom = (TensorField)From;
            Tensor derivedTo = To;
            IndexGenerator? generator = null;

            for (int i = derivativeOrders.Length - 1; i >= 0; --i)
            {
                int order = derivativeOrders[i] - _orders[i];
                while (order > 0)
                {
                    SimpleTensor variable = (SimpleTensor)From[i];
                    int[] indices = new int[variable.Indices.Size()];

                    if (indices.Length != 0 && generator is null)
                    {
                        HashSet<int> forbidden = new(iterator.GetForbidden());
                        forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(From));
                        forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(To));
                        generator = new IndexGenerator(forbidden.ToArray());
                    }

                    for (int j = indices.Length - 1; j >= 0; --j)
                    {
                        int index = variable.Indices[j];
                        indices[j] = IndicesUtils.SetRawState(
                            IndicesUtils.GetRawStateInt(index),
                            generator!.Generate(IndicesUtils.GetType(index)));
                    }

                    SimpleIndices variableIndices = IndicesFactory.CreateSimple(null, indices);
                    variable = SetIndices(variable, variableIndices);
                    derivedFrom = CreateFieldDerivative(
                        derivedFrom,
                        IndicesFactory.CreateSimple(null, variableIndices.GetInverted()),
                        i);
                    derivedTo = new DifferentiateTransformation(variable).Transform(derivedTo);
                    --order;
                }
            }

            fromTo = new DFromTo(derivedFrom, derivedTo);
            _derivatives[key] = fromTo;
        }

        return NewToInternal(fromTo, currentField, currentNode, iterator);
    }

    private Tensor NewToInternal(DFromTo fromTo, TensorField currentField, Tensor currentNode, SubstitutionIterator iterator)
    {
        ArgumentNullException.ThrowIfNull(fromTo);
        ArgumentNullException.ThrowIfNull(currentField);
        ArgumentNullException.ThrowIfNull(currentNode);
        ArgumentNullException.ThrowIfNull(iterator);

        TensorField fromField = fromTo.From;
        Mapping? mapping = IndexMappings.SimpleTensorsPort(fromField, currentField).Take();
        if (mapping is null)
        {
            return currentNode;
        }

        SimpleIndices[] fromIndices = fromField.GetArgIndices();
        SimpleIndices[] currentIndices = currentField.GetArgIndices();

        List<Tensor> argFrom = [];
        List<Tensor> argTo = [];
        for (int i = fromField.Size - 1; i >= 0; --i)
        {
            if (IndexMappings.PositiveMappingExists(currentNode[i], fromField[i]))
            {
                continue;
            }

            int[] fromArgumentIndices = fromIndices[i].AllIndices.ToArray();
            int[] currentArgumentIndices = currentIndices[i].AllIndices.ToArray();
            Tensor mappedArgument = ApplyIndexMapping.Apply(
                fromField[i],
                new Mapping(fromArgumentIndices, currentArgumentIndices),
                []);

            argFrom.Add(mappedArgument);
            argTo.Add(currentNode[i]);
        }

        Tensor newTo = fromTo.To;
        newTo = new SubstitutionTransformation([.. argFrom], [.. argTo], false).Transform(newTo);
        return ApplyIndexMappingToTo(currentNode, newTo, mapping, iterator);
    }

    private static TensorField CreateFieldDerivative(TensorField parent, SimpleIndices derivativeIndices, int argumentPosition)
    {
        ArgumentNullException.ThrowIfNull(parent);
        ArgumentNullException.ThrowIfNull(derivativeIndices);

        int[] orders = new int[parent.Size];
        orders[argumentPosition] = 1;

        NameDescriptorForTensorField derivativeDescriptor = parent.GetNameDescriptor().GetDerivative(orders);
        IndicesBuilder indicesBuilder = new();
        indicesBuilder.Append(parent.SimpleIndices);
        indicesBuilder.Append(derivativeIndices);

        SimpleIndices indices = IndicesFactory.CreateSimple(derivativeDescriptor.GetSymmetries(), indicesBuilder.Indices);
        return TensorField.Create(derivativeDescriptor.Id, indices, parent.GetArgIndices(), parent.GetArguments());
    }

    private static SimpleTensor SetIndices(SimpleTensor tensor, SimpleIndices indices)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indices);

        if (tensor is TensorField field)
        {
            return TensorField.Create(field.Name, indices, field.GetArgIndices(), field.GetArguments());
        }

        return TensorApi.SimpleTensor(tensor.Name, indices);
    }

    private sealed class DFromTo
    {
        public DFromTo(TensorField from, Tensor to)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
        }

        public TensorField From { get; }

        public Tensor To { get; }
    }
}
