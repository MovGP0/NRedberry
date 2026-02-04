using System.Linq;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Skeleton port of cc.redberry.physics.oneloopdiv.Averaging.
/// </summary>
public sealed class Averaging : ITransformation
{
    private readonly SimpleTensor constantN;

    public Averaging(SimpleTensor constantN)
    {
        ArgumentNullException.ThrowIfNull(constantN);
        this.constantN = constantN;
    }

    private static Tensor Average(int[] indices)
    {
        if (indices.Length == 0)
        {
            return Complex.One;
        }

        if (indices.Length == 2)
        {
            return Context.Get().CreateMetricOrKronecker(indices[0], indices[1]);
        }

        SumBuilder sumBuilder = new();
        for (int i = 1; i < indices.Length; ++i)
        {
            int[] suffix = new int[indices.Length - 2];
            Array.Copy(indices, 1, suffix, 0, i - 1);
            Array.Copy(indices, i + 1, suffix, i - 1, indices.Length - i - 1);
            sumBuilder.Put(Tensors.Tensors.Multiply(Context.Get().CreateMetricOrKronecker(indices[0], indices[i]), Average(suffix)));
        }

        return sumBuilder.Build();
    }

    public Tensor Transform(Tensor tensor)
    {
        if (tensor is Sum or Expression)
        {
            Tensor[] newSumElements = new Tensor[tensor.Size];
            bool needRebuild = false;
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor tensorCurrent = tensor[i];
                Tensor tempResult = Transform(tensorCurrent);
                if (!ReferenceEquals(tensorCurrent, tempResult))
                {
                    needRebuild = true;
                }

                newSumElements[i] = tempResult;
            }

            if (needRebuild)
            {
                TensorFactory? factory = tensor.GetFactory();
                return factory is null ? tensor : factory.Create(newSumElements);
            }

            return tensor;
        }

        if (tensor is Product)
        {
            int count = 0;
            IndicesBuilder indicesBuilder = new();
            List<Tensor> newProductElements = [];
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor current = tensor[i];
                if (IsN(current))
                {
                    indicesBuilder.Append(current);
                    ++count;
                    continue;
                }

                if (TensorUtils.IsScalar(current))
                {
                    FromChildToParentIterator iterator = new(current);
                    Tensor? temp;
                    bool foundN = false;
                    while ((temp = iterator.Next()) is not null)
                    {
                        if (IsN(temp))
                        {
                            foundN = true;
                            break;
                        }
                    }

                    if (!foundN)
                    {
                        newProductElements.Add(current);
                        continue;
                    }

                    if (current is not Power
                        || !TensorUtils.IsInteger(current[1])
                        || current[1] is not Complex exponent
                        || exponent.IntValue() != 2)
                    {
                        throw new ArgumentException();
                    }

                    Tensor[] bases = [current[0], current[0]];
                    bases[1] = ApplyIndexMapping.RenameDummy(
                        bases[1],
                        TensorUtils.GetAllIndicesNamesT(tensor).ToArray());
                    foundN = false;
                    foreach (Tensor baseTensor in bases)
                    {
                        foreach (Tensor t in baseTensor)
                        {
                            if (IsN(t))
                            {
                                indicesBuilder.Append(t);
                                ++count;
                                foundN = true;
                            }
                            else
                            {
                                newProductElements.Add(t);
                            }
                        }
                    }

                    if (!foundN)
                    {
                        throw new ArgumentException("Expand first");
                    }
                }
                else
                {
                    newProductElements.Add(current);
                }
            }

            if (count == 0)
            {
                return tensor;
            }

            if (count % 2 != 0)
            {
                return Complex.Zero;
            }

            count /= 2;
            Tensor result = Average(indicesBuilder.Indices.AllIndices.ToArray());
            long factor = Pow(2, count) * Factorial(count + 1);
            Complex number = new Complex(factor).Reciprocal();
            result = ExpandTransformation.Expand(result);
            newProductElements.Add(number);
            newProductElements.Add(result);
            return Tensors.Tensors.Multiply(newProductElements.ToArray());
        }

        if (tensor is Power)
        {
            Tensor nBase = Transform(tensor[0]);
            if (ReferenceEquals(nBase, tensor[0]))
            {
                return tensor;
            }

            return Tensors.Tensors.Pow(nBase, tensor[1]);
        }

        return tensor;
    }

    private bool IsN(Tensor tensor)
    {
        return tensor is SimpleTensor simpleTensor && simpleTensor.Name == constantN.Name;
    }

    private static long Pow(long value, int power)
    {
        long result = 1;
        for (int i = 0; i < power; ++i)
        {
            result *= value;
        }

        return result;
    }

    private static long Factorial(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        long result = 1;
        for (int i = 2; i <= value; ++i)
        {
            result *= i;
        }

        return result;
    }
}
