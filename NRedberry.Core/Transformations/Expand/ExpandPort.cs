using NRedberry.Concurrent;
using NRedberry.Core.Combinatorics;
using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandPort.
/// </summary>
public static class ExpandPort
{
    public static Tensor ExpandUsingPort(Tensor tensor)
    {
        return ExpandUsingPort(tensor, true);
    }

    public static Tensor ExpandUsingPort(Tensor tensor, bool expandSymbolic)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        IOutputPort<Tensor> port = CreatePort(tensor, expandSymbolic);
        if (port is SingletonPort singleton)
        {
            return singleton.Take();
        }

        SumBuilder sumBuilder = new();
        Tensor? current;
        while ((current = port.Take()) is not null)
        {
            sumBuilder.Put(current);
        }

        return sumBuilder.Build();
    }

    public static IOutputPort<Tensor> CreatePort(Tensor tensor, bool expandSymbolic)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        if (tensor is Product)
        {
            return new ProductPort(tensor, expandSymbolic);
        }

        if (tensor is Sum)
        {
            return new SumPort(tensor, expandSymbolic);
        }

        if (ExpandUtils.IsExpandablePower(tensor)
            && !TensorUtils.IsNegativeNaturalNumber(tensor[1]))
        {
            if (!expandSymbolic && TensorUtils.IsSymbolic(tensor[0]))
            {
                return new SingletonPort(tensor);
            }

            return new PowerPort(tensor, expandSymbolic);
        }

        return new SingletonPort(tensor);
    }

    public sealed class ExpandPairPort : IOutputPort<Tensor>
    {
        private readonly Tensor sum1;
        private readonly Tensor sum2;
        private readonly Tensor[] factors;
        private long index;

        public ExpandPairPort(Sum first, Sum second)
        {
            sum1 = first;
            sum2 = second;
            factors = Array.Empty<Tensor>();
        }

        public ExpandPairPort(Sum first, Sum second, Tensor[] factors)
        {
            sum1 = first;
            sum2 = second;
            this.factors = factors ?? throw new ArgumentNullException(nameof(factors));
        }

        public Tensor Take()
        {
            long total = (long)sum1.Size * sum2.Size;
            if (index >= total)
            {
                return null!;
            }

            int firstIndex = (int)(index / sum2.Size);
            int secondIndex = (int)(index % sum2.Size);
            index++;
            if (factors.Length == 0)
            {
                return Tensors.Tensors.Multiply(sum1[firstIndex], sum2[secondIndex]);
            }

            return Tensors.Tensors.Multiply(
                TensorArrayUtils.AddAll(factors, sum1[firstIndex], sum2[secondIndex]));
        }
    }

    private interface IResettablePort : IOutputPort<Tensor>
    {
        void Reset();
    }

    private sealed class SingletonPort : IOutputPort<Tensor>
    {
        private readonly Tensor tensor;
        private bool taken;

        public SingletonPort(Tensor tensor)
        {
            this.tensor = tensor ?? throw new ArgumentNullException(nameof(tensor));
        }

        public Tensor Take()
        {
            if (taken)
            {
                return null!;
            }

            taken = true;
            return tensor;
        }
    }

    private sealed class PowerPort : IResettablePort
    {
        private readonly Tensor baseTensor;
        private readonly int power;
        private IntTuplesPort tuplesPort;
        private readonly int[] initialForbidden;
        private IOutputPort<Tensor>? currentPort;
        private readonly bool expandSymbolic;

        public PowerPort(Tensor tensor, int[] initialForbidden, bool expandSymbolic)
        {
            this.expandSymbolic = expandSymbolic;
            baseTensor = tensor[0];
            power = ((Complex)tensor[1]).IntValue();
            var upperBounds = new int[power];
            Array.Fill(upperBounds, baseTensor.Size);
            tuplesPort = new IntTuplesPort(upperBounds);
            this.initialForbidden = initialForbidden;
            currentPort = NextPort();
        }

        public PowerPort(Tensor tensor, bool expandSymbolic)
            : this(tensor, ToArray(TensorUtils.GetAllIndicesNamesT(tensor[0])), expandSymbolic)
        {
        }

        private IOutputPort<Tensor>? NextPort()
        {
            int[]? tuple = tuplesPort.Take();
            if (tuple is null)
            {
                return null;
            }

            HashSet<int> added = new(initialForbidden);
            TensorBuilder builder = new ScalarsBackedProductBuilder();
            builder.Put(baseTensor[tuple[0]]);
            for (int i = 1; i < tuple.Length; ++i)
            {
                builder.Put(
                    ApplyIndexMapping.RenameDummy(
                        baseTensor[tuple[i]],
                        ToArray(added),
                        added));
            }

            return CreatePort(builder.Build(), expandSymbolic);
        }

        public Tensor Take()
        {
            if (currentPort is null)
            {
                return null!;
            }

            Tensor result = currentPort.Take();
            if (result is null)
            {
                currentPort = NextPort();
                return Take();
            }

            return result;
        }

        public void Reset()
        {
            tuplesPort.Reset();
            currentPort = NextPort();
        }
    }

    private sealed class ProductPort : IOutputPort<Tensor>
    {
        private readonly TensorBuilder baseBuilder;
        private TensorBuilder? currentBuilder;
        private readonly IResettablePort[] sumsAndPowers;
        private readonly Tensor[] currentMultipliers;
        private readonly Tensor tensor;
        private readonly bool expandSymbolic;

        public ProductPort(Tensor tensor, bool expandSymbolic)
        {
            this.tensor = tensor;
            this.expandSymbolic = expandSymbolic;
            baseBuilder = new ScalarsBackedProductBuilder();
            List<IResettablePort> sumOrPowerPorts = new();
            int theLargestSumPosition = 0;
            int theLargestSumSize = 0;
            int productSize = tensor.Size;
            for (int i = 0; i < productSize; ++i)
            {
                Tensor multiplier = tensor[i];
                if (multiplier is Sum && ExpandIfSymbolic(multiplier))
                {
                    if (multiplier.Size > theLargestSumSize)
                    {
                        theLargestSumPosition = sumOrPowerPorts.Count;
                        theLargestSumSize = multiplier.Size;
                    }

                    sumOrPowerPorts.Add(new SumPort(multiplier, expandSymbolic));
                }
                else if (ExpandUtils.IsExpandablePower(multiplier))
                {
                    if (TensorUtils.IsNegativeNaturalNumber(multiplier[1]) || !ExpandIfSymbolic(multiplier[0]))
                    {
                        baseBuilder.Put(multiplier);
                        continue;
                    }

                    var baseSize = new System.Numerics.BigInteger(multiplier[0].Size);
                    var exponent = new System.Numerics.BigInteger(((Complex)multiplier[1]).IntValue());
                    if (NumberUtils.Pow(baseSize, exponent)
                        .CompareTo(new System.Numerics.BigInteger(theLargestSumSize)) > 0)
                    {
                        theLargestSumPosition = sumOrPowerPorts.Count;
                        theLargestSumSize = multiplier.Size;
                    }

                    sumOrPowerPorts.Add(
                        new PowerPort(multiplier, ToArray(TensorUtils.GetAllIndicesNamesT(tensor)), expandSymbolic));
                }
                else
                {
                    baseBuilder.Put(multiplier);
                }
            }

            sumsAndPowers = sumOrPowerPorts.ToArray();
            if (sumsAndPowers.Length <= 1)
            {
                currentMultipliers = Array.Empty<Tensor>();
                currentBuilder = baseBuilder;
            }
            else
            {
                IResettablePort temp = sumsAndPowers[theLargestSumPosition];
                sumsAndPowers[theLargestSumPosition] = sumsAndPowers[^1];
                sumsAndPowers[^1] = temp;

                currentMultipliers = new Tensor[sumsAndPowers.Length - 2];
                for (productSize = 0; productSize < sumsAndPowers.Length - 2; ++productSize)
                {
                    currentMultipliers[productSize] = sumsAndPowers[productSize].Take();
                }

                currentBuilder = NextCombination();
            }
        }

        private bool ExpandIfSymbolic(Tensor tensor)
        {
            return expandSymbolic || !TensorUtils.IsSymbolic(tensor);
        }

        private TensorBuilder? NextCombination()
        {
            if (sumsAndPowers.Length == 1)
            {
                return null;
            }

            int pointer = sumsAndPowers.Length - 2;
            TensorBuilder temp = baseBuilder.Clone();
            bool next = false;
            Tensor? current = sumsAndPowers[pointer].Take();
            if (current is null)
            {
                sumsAndPowers[pointer].Reset();
                current = sumsAndPowers[pointer].Take();
                next = true;
            }

            temp.Put(current);
            while (--pointer >= 0)
            {
                if (next)
                {
                    next = false;
                    current = sumsAndPowers[pointer].Take();
                    if (current is null)
                    {
                        sumsAndPowers[pointer].Reset();
                        current = sumsAndPowers[pointer].Take();
                        next = true;
                    }

                    currentMultipliers[pointer] = current;
                }

                temp.Put(currentMultipliers[pointer]);
            }

            if (next)
            {
                return null;
            }

            return temp;
        }

        public Tensor Take()
        {
            if (currentBuilder is null)
            {
                return null!;
            }

            if (sumsAndPowers.Length == 0)
            {
                currentBuilder = null;
                return tensor;
            }

            Tensor current = sumsAndPowers[^1].Take();
            if (current is null)
            {
                currentBuilder = NextCombination();
                sumsAndPowers[^1].Reset();
                return Take();
            }

            TensorBuilder temp = currentBuilder.Clone();
            temp.Put(current);
            return temp.Build();
        }
    }

    private sealed class SumPort : IResettablePort
    {
        private readonly IOutputPort<Tensor>[] ports;
        private readonly Tensor tensor;
        private int pointer;
        private readonly bool expandSymbolic;

        public SumPort(Tensor tensor, bool expandSymbolic)
        {
            this.tensor = tensor;
            this.expandSymbolic = expandSymbolic;
            ports = new IOutputPort<Tensor>[tensor.Size];
            Reset();
        }

        public void Reset()
        {
            pointer = 0;
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                ports[i] = CreatePort(tensor[i], expandSymbolic);
            }
        }

        public Tensor Take()
        {
            Tensor? current = null;
            while (pointer < tensor.Size)
            {
                current = ports[pointer].Take();
                if (current is null)
                {
                    pointer++;
                }
                else
                {
                    return current;
                }
            }

            return current!;
        }
    }

    private static int[] ToArray(HashSet<int> set)
    {
        int[] result = new int[set.Count];
        int i = 0;
        foreach (int value in set)
        {
            result[i++] = value;
        }

        return result;
    }
}
