using System.Text;
using NRedberry.Contexts;
using NRedberry.Indices;

namespace NRedberry.Tensors;

public sealed class TensorField : SimpleTensor
{
    private Tensor[] Args { get; }
    private SimpleIndices[] ArgIndices { get; }

    internal Tensor[] Arguments => Args;
    internal SimpleIndices[] ArgumentIndices => ArgIndices;

    internal TensorField(int name, SimpleIndices indices, Tensor[] args, SimpleIndices[] argIndices)
        : base(name, indices)
    {
        ArgumentNullException.ThrowIfNull(args);
        ArgumentNullException.ThrowIfNull(argIndices);

        Args = args;
        ArgIndices = argIndices;
    }

    internal TensorField(TensorField field, Tensor[] args)
        : base(field.Name, field.SimpleIndices)
    {
        ArgumentNullException.ThrowIfNull(field);
        ArgumentNullException.ThrowIfNull(args);

        Args = args;
        ArgIndices = field.ArgIndices;
    }

    public Tensor[] GetArguments()
    {
        return (Tensor[])Args.Clone();
    }

    public SimpleIndices[] GetArgIndices()
    {
        return (SimpleIndices[])ArgIndices.Clone();
    }

    public SimpleIndices GetArgIndices(int i)
    {
        return ArgIndices[i];
    }

    public bool IsDerivative()
    {
        return GetNameDescriptor().IsDerivative();
    }

    public bool IsDiracDelta()
    {
        return GetNameDescriptor().IsDiracDelta;
    }

    public override Tensor this[int i] => Args[i];

    public override int Size => Args.Length;

    public override IEnumerator<Tensor> GetEnumerator()
    {
        return new BasicTensorIterator(this);
    }

    public new NameDescriptorForTensorField GetNameDescriptor()
    {
        return (NameDescriptorForTensorField)base.GetNameDescriptor();
    }

    public TensorField GetParentField()
    {
        if (!IsDerivative())
        {
            return this;
        }

        NameDescriptorForTensorField parent = GetNameDescriptor().GetParent();
        SimpleIndices[][] partition = GetPartitionOfIndices();
        return Create(parent.Id, partition[0][0], Args);
    }

    public int GetDerivativeOrder(int i)
    {
        return GetNameDescriptor().GetDerivativeOrder(i);
    }

    public SimpleIndices[][] GetPartitionOfIndices()
    {
        NameDescriptorForTensorField fieldDescriptor = GetNameDescriptor();
        if (!fieldDescriptor.IsDerivative())
        {
            var ret = new SimpleIndices[Args.Length + 1][];
            Array.Fill(ret, Array.Empty<SimpleIndices>(), 1, ret.Length - 1);
            ret[0] = [SimpleIndices];
            return ret;
        }

        int[] orders = fieldDescriptor.GetDerivativeOrders();
        int[][] mapping = fieldDescriptor.GetIndicesPartitionMapping();
        var partition = new SimpleIndices[Args.Length + 1][];
        int totalOrder = 0;
        for (int i = 0; i <= Args.Length; ++i)
        {
            int count = i == 0 ? 1 : orders[i - 1];
            partition[i] = new SimpleIndices[count];
            for (int j = 0; j < count; ++j)
            {
                int[] map = mapping[totalOrder++];
                int[] data = new int[map.Length];
                for (int k = 0; k < map.Length; ++k)
                {
                    data[k] = SimpleIndices[map[k]];
                }

                partition[i][j] = IndicesFactory.CreateSimple(null, data);
            }
        }

        return partition;
    }

    public override string ToString(OutputFormat outputFormat)
    {
        return ToString(outputFormat, null);
    }

    protected override string ToString<T>(OutputFormat outputFormat)
    {
        return ToString(outputFormat, typeof(T));
    }

    public override TensorBuilder GetBuilder()
    {
        return new TensorFieldBuilder(this);
    }

    public override TensorFactory GetFactory()
    {
        return new TensorFieldFactory(this);
    }

    private string ToString(OutputFormat outputFormat, Type? clazz)
    {
        if (!outputFormat.PrintMatrixIndices
            && Context.Get().IsKronecker(this)
            && !CC.IsMetric(IndicesUtils.GetType_(SimpleIndices[0])))
        {
            string str = clazz == typeof(Sum) ? "1" : string.Empty;
            return SimpleIndices.GetFree().Size() == 0 ? ToString0(outputFormat) : str;
        }

        if (!outputFormat.PrintMatrixIndices)
        {
            if (clazz != typeof(Product))
            {
                HashSet<IndexType> matrixTypes = IndicesUtils.NonMetricTypes(SimpleIndices);
                if (matrixTypes.Count == 0)
                {
                    return ToString0(outputFormat);
                }

                List<IndexType> traces = [];
                foreach (IndexType type in matrixTypes)
                {
                    Indices.Indices ofType = SimpleIndices.GetOfType(type);
                    if (ofType.GetFree().Size() == 0)
                    {
                        traces.Add(type);
                    }
                }

                if (traces.Count != 0)
                {
                    var sb = new StringBuilder();
                    sb.Append("Tr[").Append(ToString0(outputFormat));
                    if (traces.Count != matrixTypes.Count)
                    {
                        sb.Append(", ");
                        for (int i = 0; ; ++i)
                        {
                            sb.Append(traces[i]);
                            if (i == traces.Count - 1)
                            {
                                break;
                            }

                            sb.Append(", ");
                        }
                    }

                    sb.Append("]");
                    return sb.ToString();
                }

                return ToString0(outputFormat);
            }

            return ToString0(outputFormat);
        }

        return ToString0(outputFormat);
    }

    private string ToString0(OutputFormat outputFormat)
    {
        var sb = new StringBuilder();
        sb.Append(CC.GetNameDescriptor(Name).GetName(SimpleIndices, outputFormat));

        if (SimpleIndices.Size() != 0)
        {
            bool external = outputFormat.Is(OutputFormat.WolframMathematica)
                || outputFormat.Is(OutputFormat.Maple);
            if (external)
            {
                sb.Append('[');
            }

            sb.Append(SimpleIndices.ToString(outputFormat));

            if (external)
            {
                sb.Append(']');
            }
        }

        if (outputFormat.Is(OutputFormat.Maple))
        {
            sb.Append('(');
        }
        else
        {
            sb.Append('[');
        }

        for (int i = 0; i < Args.Length; ++i)
        {
            sb.Append(Args[i].ToString(outputFormat));
            sb.Append(',');
        }

        if (Args.Length > 0)
        {
            sb.Length--;
        }

        if (outputFormat.Is(OutputFormat.Maple))
        {
            sb.Append(')');
        }
        else
        {
            sb.Append(']');
        }

        return sb.ToString();
    }

    internal static TensorField Create(string name, SimpleIndices indices, Tensor[] arguments)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(arguments);

        var argIndices = new SimpleIndices[arguments.Length];
        for (int i = 0; i < arguments.Length; ++i)
        {
            argIndices[i] = IndicesFactory.CreateSimple(null, arguments[i].Indices.GetFree());
        }

        return Create(name, indices, argIndices, arguments);
    }

    internal static TensorField Create(string name, SimpleIndices indices, SimpleIndices[] argIndices, Tensor[] arguments)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(argIndices);
        ArgumentNullException.ThrowIfNull(arguments);

        if (argIndices.Length != arguments.Length)
        {
            throw new ArgumentException("Argument indices array and arguments array have different length.");
        }

        if (arguments.Length == 0)
        {
            throw new ArgumentException("No arguments in field.");
        }

        for (int i = 0; i < argIndices.Length; ++i)
        {
            if (!arguments[i].Indices.GetFree().EqualsRegardlessOrder(argIndices[i]))
            {
                throw new ArgumentException("Arguments indices are inconsistent with arguments.");
            }
        }

        var structures = new StructureOfIndices[argIndices.Length + 1];
        structures[0] = indices.StructureOfIndices;
        for (int i = 0; i < argIndices.Length; ++i)
        {
            structures[i + 1] = argIndices[i].StructureOfIndices;
        }

        NameDescriptor descriptor = CC.NameManager.MapNameDescriptor(name, structures);
        SimpleIndices tensorIndices = IndicesFactory.CreateSimple(descriptor.GetSymmetries(), indices);
        return new TensorField(descriptor.Id, tensorIndices, arguments, argIndices);
    }

    internal static TensorField Create(int name, SimpleIndices indices, Tensor[] arguments)
    {
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(arguments);

        if (arguments.Length == 0)
        {
            throw new ArgumentException("No arguments in field.");
        }

        NameDescriptor descriptor = CC.GetNameDescriptor(name);
        if (descriptor is null)
        {
            throw new ArgumentException("This name is not registered in the system.");
        }

        if (!descriptor.GetStructureOfIndices().IsStructureOf(indices))
        {
            throw new ArgumentException("Specified indices are not indices of specified tensor.");
        }

        var argIndices = new SimpleIndices[arguments.Length];
        for (int i = 0; i < arguments.Length; ++i)
        {
            argIndices[i] = IndicesFactory.CreateSimple(null, arguments[i].Indices.GetFree());
        }

        return Create(name, indices, argIndices, arguments);
    }

    internal static TensorField Create(int name, SimpleIndices indices, SimpleIndices[] argIndices, Tensor[] arguments)
    {
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(argIndices);
        ArgumentNullException.ThrowIfNull(arguments);

        if (argIndices.Length != arguments.Length)
        {
            throw new ArgumentException("Argument indices array and arguments array have different length.");
        }

        if (arguments.Length == 0)
        {
            throw new ArgumentException("No arguments in field.");
        }

        NameDescriptor descriptor = CC.GetNameDescriptor(name);
        if (descriptor is null)
        {
            throw new ArgumentException("This name is not registered in the system.");
        }

        if (!descriptor.IsField())
        {
            throw new ArgumentException("Name correspods to simple tensor (not a field).");
        }

        if (descriptor.GetStructuresOfIndices().Length - 1 != argIndices.Length)
        {
            throw new ArgumentException("This name corresponds to field with different number of arguments.");
        }

        if (!descriptor.GetStructureOfIndices().IsStructureOf(indices))
        {
            throw new ArgumentException("Specified indices are not indices of specified tensor.");
        }

        for (int i = 0; i < argIndices.Length; ++i)
        {
            if (!descriptor.GetStructuresOfIndices()[i + 1].IsStructureOf(argIndices[i]))
            {
                throw new ArgumentException("Arguments indices are inconsistent with field signature.");
            }

            if (!arguments[i].Indices.GetFree().EqualsRegardlessOrder(argIndices[i]))
            {
                throw new ArgumentException("Arguments indices are inconsistent with arguments.");
            }
        }

        SimpleIndices tensorIndices = IndicesFactory.CreateSimple(descriptor.GetSymmetries(), indices);
        return new TensorField(name, tensorIndices, arguments, argIndices);
    }
}
