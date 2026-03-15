using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using NRedberry.Tensors;
using NumberComplex = NRedberry.Numbers.Complex;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Groovy;

public static class Redberry
{
    extension(string value)
    {
        public Tensor t => TensorApi.Parse(value);
    }

    extension(Tensor value)
    {
        public Tensor t => value;
    }

    extension(bool value)
    {
        public bool t => value;
    }

    extension(sbyte value)
    {
        public NumberComplex t => new(value);
    }

    extension(byte value)
    {
        public NumberComplex t => new((BigInteger)value);
    }

    extension(short value)
    {
        public NumberComplex t => new(value);
    }

    extension(ushort value)
    {
        public NumberComplex t => new((BigInteger)value);
    }

    extension(int value)
    {
        public NumberComplex t => new(value);
    }

    extension(uint value)
    {
        public NumberComplex t => new((BigInteger)value);
    }

    extension(long value)
    {
        public NumberComplex t => new(value);
    }

    extension(ulong value)
    {
        public NumberComplex t => new((BigInteger)value);
    }

    extension(float value)
    {
        public NumberComplex t => new(value);
    }

    extension(double value)
    {
        public NumberComplex t => new(value);
    }

    extension(decimal value)
    {
        public NumberComplex t => new((double)value);
    }

    extension(BigInteger value)
    {
        public NumberComplex t => new(value);
    }

    extension(Permutation value)
    {
        public Permutation p => value;
    }

    extension(IEnumerable<int> oneLine)
    {
        public Permutation p => CreateOneLinePermutation(oneLine);
    }

    extension(IEnumerable<IEnumerable<int>> cycles)
    {
        public Permutation p => CreateCyclePermutation(cycles);
    }

    private static Permutation CreateOneLinePermutation(IEnumerable<int> values)
    {
        int[] oneLine = values.ToArray();
        bool antisymmetry = oneLine.Any(static value => value < 0);
        if (antisymmetry)
        {
            oneLine = oneLine.Select(static value => -value).ToArray();
        }

        return Permutations.CreatePermutation(antisymmetry, oneLine);
    }

    private static Permutation CreateCyclePermutation(IEnumerable<IEnumerable<int>> cycles)
    {
        int[][] cycleArray = cycles.Select(static cycle => cycle.ToArray()).ToArray();
        bool antisymmetry = cycleArray.SelectMany(static cycle => cycle).Any(static value => value < 0);
        if (antisymmetry)
        {
            cycleArray = cycleArray
                .Select(static cycle => cycle.Select(static value => -value).ToArray())
                .ToArray();
        }

        return Permutations.CreatePermutation(antisymmetry, cycleArray);
    }
}
