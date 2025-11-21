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

    /**
         * Returns the name (unique identifier) of this tensor
         *
         * @return name of this tensor
         * @see cc.redberry.core.context.NameDescriptor
         */
    public override int GetHashCode()
    {
        return Name;
    }

    public override Tensor this[int i] => throw new IndexOutOfRangeException();

    public override int Size => 0;

    public override string ToString(OutputFormat outputFormat)
    {
        var sb = new StringBuilder();
        sb.Append(CC.GetNameDescriptor(Name).GetName(SimpleIndices, outputFormat));
        sb.Append(Indices.ToString(outputFormat));
        return sb.ToString();
    }

    public override IEnumerator<Tensor> GetEnumerator()
    {
        yield break;
    }

    public override TensorBuilder GetBuilder()
    {
        return new SimpleTensorBuilder(this);
    }

    public override TensorFactory GetFactory()
    {
        return new SimpleTensorFactory(this);
    }

    public NameDescriptor GetNameDescriptor()
    {
        return ContextManager.CurrentContext.GetNameDescriptor(Name);
    }

    public string GetStringName(OutputFormat outputFormat)
    {
        return CC.Current
            .GetNameDescriptor(Name)
            .GetName(SimpleIndices, outputFormat);
    }

    public bool Equals(SimpleTensor? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((SimpleTensor) obj);
    }
}
