using System.Text;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Tensors.Functions;

namespace NRedberry.Tensors;

public class SimpleTensor(int name, SimpleIndices indices) : Tensor, IEquatable<SimpleTensor>
{
    public SimpleIndices SimpleIndices { get; } = indices ?? throw new ArgumentNullException(nameof(indices));
    public override Indices.Indices Indices => SimpleIndices;

    /// <summary>
    /// Returns the name (unique identifier) of this tensor.
    /// </summary>
    /// <returns>
    /// name of this tensor
    /// </returns>
    /// <seealso cref="NameDescriptor"/>
    public int Name { get; } = name;

    public override int GetHashCode()
    {
        return Name;
    }

    public override Tensor this[int i] => throw new IndexOutOfRangeException();

    public override int Size => 0;

    public override string ToString(OutputFormat outputFormat)
    {
        return ToString(outputFormat, null);
    }

    protected override string ToString<T>(OutputFormat outputFormat)
    {
        return ToString(outputFormat, typeof(T));
    }

    public override IEnumerator<Tensor> GetEnumerator()
    {
        yield break;
    }

    public override TensorBuilder GetBuilder()
    {
        return new SimpleTensorBuilder(this);
    }

    public override TensorFactory? GetFactory()
    {
        return new SimpleTensorFactory(this);
    }

    public NameDescriptor GetNameDescriptor()
    {
        return ContextManager.CurrentContext.GetNameDescriptor(Name);
    }

    public string GetStringName(OutputFormat outputFormat)
    {
        return CC.GetNameDescriptor(Name).GetName(SimpleIndices, outputFormat);
    }

    public string GetStringName()
    {
        return GetStringName(CC.GetDefaultOutputFormat());
    }

    public bool Equals(SimpleTensor? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name && SimpleIndices.Equals(other.SimpleIndices);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((SimpleTensor)obj);
    }

    public static bool operator ==(SimpleTensor? left, SimpleTensor? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(SimpleTensor? left, SimpleTensor? right)
    {
        return !(left == right);
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

        if (SimpleIndices.Size() == 0)
        {
            return sb.ToString();
        }

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

        return sb.ToString();
    }
}
