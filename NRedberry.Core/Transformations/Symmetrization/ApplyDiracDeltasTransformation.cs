using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Substitutions;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.ApplyDiracDeltasTransformation.
/// </summary>
public sealed class ApplyDiracDeltasTransformation : TransformationToStringAble
{
    public static ApplyDiracDeltasTransformation Instance { get; } = new();

    private ApplyDiracDeltasTransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (!ContainsDiracDeltas(tensor))
        {
            return tensor;
        }

        IOutputPort<Tensor> port = ExpandPort.CreatePort(tensor, true);
        SumBuilder sumBuilder = new();

        Tensor? current;
        while ((current = port.Take()) is not null)
        {
            if (current is Product)
            {
                while (true)
                {
                    if (current is not Product product)
                    {
                        break;
                    }

                    bool updated = false;
                    for (int i = 0; i < product.Size; ++i)
                    {
                        Tensor dd = current[i];
                        if (dd is TensorField field && field.IsDiracDelta())
                        {
                            Tensor temp = product.Remove(i);
                            temp = CreateSubstitution(field).Transform(temp);
                            if (!Intersects(TensorUtils.GetSimpleTensorsNames(temp), TensorUtils.GetSimpleTensorsNames(field[0])))
                            {
                                current = temp;
                                updated = current is Product;
                                break;
                            }
                        }
                    }

                    if (!updated)
                    {
                        break;
                    }
                }
            }

            sumBuilder.Put(current);
        }

        return sumBuilder.Build();
    }

    private static bool ContainsDiracDeltas(Tensor tensor)
    {
        if (tensor is TensorField field && field.IsDiracDelta())
        {
            if (tensor[0] is SimpleTensor)
            {
                return true;
            }

            if (tensor[0] is Product product)
            {
                return product.Data.Length == 1 && product.Data[0] is SimpleTensor;
            }

            return false;
        }

        foreach (Tensor child in tensor)
        {
            if (ContainsDiracDeltas(child))
            {
                return true;
            }
        }

        return false;
    }

    private static ITransformation CreateSubstitution(TensorField delta)
    {
        Tensor from = delta[0];
        Tensor to = delta[1];
        to = ApplyIndexMapping.Apply(
            to,
            new Mapping(delta.GetArgIndices(1).AllIndices.ToArray(), delta.GetArgIndices(0).AllIndices.ToArray()),
            Array.Empty<int>());
        return new SubstitutionTransformation(from, to);
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "ApplyDiracDeltas";
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }

    private static bool Intersects(HashSet<int> first, HashSet<int> second)
    {
        if (second.Count > first.Count)
        {
            return Intersects(second, first);
        }

        foreach (int item in second)
        {
            if (first.Contains(item))
            {
                return true;
            }
        }

        return false;
    }
}
