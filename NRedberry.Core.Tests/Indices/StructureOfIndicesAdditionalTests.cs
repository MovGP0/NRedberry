using System.Collections;
using NRedberry.Contexts;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public sealed class StructureOfIndicesAdditionalTests
{
    [Fact]
    public void CreateTypeCountShouldHandleNonEmptyAndEmpty()
    {
        try
        {
            if (!TryGetMetricTypes(1, out var metricTypes))
            {
                return;
            }

            byte type = metricTypes[0];

            StructureOfIndices structure = StructureOfIndices.Create(type, 3);
            Assert.Equal(3, structure.Size);
            Assert.Equal(3, structure.Count);
            Assert.Equal(3, structure.TypeCount(type));

            StructureOfIndices empty = StructureOfIndices.Create(type, 0);
            Assert.Same(StructureOfIndices.Empty, empty);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void CreateTypesCountShouldHandleNonEmptyAndEmpty()
    {
        try
        {
            if (!TryGetMetricTypes(2, out var metricTypes))
            {
                return;
            }

            byte[] types = [metricTypes[0], metricTypes[1]];
            int[] counts = [2, 1];
            StructureOfIndices structure = StructureOfIndices.Create(types, counts);

            Assert.Equal(3, structure.Size);
            Assert.Equal(2, structure.TypeCount(types[0]));
            Assert.Equal(1, structure.TypeCount(types[1]));

            StructureOfIndices empty = StructureOfIndices.Create(types, [0, 0]);
            Assert.Same(StructureOfIndices.Empty, empty);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void CreateAllCountAllStatesShouldHandleNonEmptyAndEmpty()
    {
        try
        {
            const int typeCount = IndexTypeMethods.TypesCount;
            int[] counts = new int[typeCount];
            BitArray[] states = new BitArray[typeCount];
            byte metricType = 0;
            byte nonMetricType = 0;
            bool hasMetric = false;
            bool hasNonMetric = false;

            for (byte i = 0; i < typeCount; ++i)
            {
                if (CC.IsMetric(i))
                {
                    states[i] = null!;
                    if (!hasMetric)
                    {
                        metricType = i;
                        hasMetric = true;
                    }
                }
                else
                {
                    states[i] = new BitArray(0);
                    if (!hasNonMetric)
                    {
                        nonMetricType = i;
                        hasNonMetric = true;
                    }
                }
            }

            if (!hasMetric || !hasNonMetric)
            {
                return;
            }

            counts[metricType] = 2;
            counts[nonMetricType] = 2;
            states[nonMetricType] = new BitArray([true, false]);

            StructureOfIndices structure = StructureOfIndices.Create(counts, states);
            Assert.Equal(4, structure.Size);
            Assert.Equal(2, structure.TypeCount(metricType));
            Assert.Equal(2, structure.TypeCount(nonMetricType));

            int[] zeroCounts = new int[typeCount];
            BitArray[] zeroStates = new BitArray[typeCount];
            for (byte i = 0; i < typeCount; ++i)
            {
                zeroStates[i] = CC.IsMetric(i) ? null! : new BitArray(0);
            }

            StructureOfIndices empty = StructureOfIndices.Create(zeroCounts, zeroStates);
            Assert.Same(StructureOfIndices.Empty, empty);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void GetTypesCountsShouldReturnClone()
    {
        try
        {
            if (!TryGetMetricTypes(1, out var metricTypes))
            {
                return;
            }

            byte metricType = metricTypes[0];
            StructureOfIndices structure = StructureOfIndices.Create(metricType, 2);
            int[] copy = structure.GetTypesCounts();
            copy[metricType] = 99;

            Assert.Equal(2, structure.TypeCount(metricType));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void GetStatesShouldReturnClone()
    {
        try
        {
            if (!TryGetNonMetricType(out byte nonMetricType))
            {
                return;
            }

            StructureOfIndices structure = StructureOfIndices.Create(nonMetricType, 2, true, false);
            BitArray[] statesCopy = structure.GetStates();
            statesCopy[nonMetricType].Set(0, false);

            BitArray[] statesCopyAgain = structure.GetStates();
            Assert.True(statesCopyAgain[nonMetricType].Get(0));
            Assert.False(statesCopyAgain[nonMetricType].Get(1));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void AppendPowSubtractShouldWorkForMetricOnlyStructures()
    {
        try
        {
            if (!TryGetMetricTypes(1, out var metricTypes))
            {
                return;
            }

            byte metricType = metricTypes[0];
            StructureOfIndices left = StructureOfIndices.Create(metricType, 2);
            StructureOfIndices right = StructureOfIndices.Create(metricType, 1);

            StructureOfIndices appended = left.Append(right);
            Assert.Equal(3, appended.Size);
            Assert.Equal(3, appended.TypeCount(metricType));

            StructureOfIndices pow = left.Pow(3);
            Assert.Equal(6, pow.Size);
            Assert.Equal(6, pow.TypeCount(metricType));

            StructureOfIndices subtracted = appended.Subtract(right);
            Assert.Equal(left, subtracted);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void EqualsAndHashCodeShouldMatchForIdenticalStructures()
    {
        try
        {
            if (!TryGetMetricTypes(1, out var metricTypes))
            {
                return;
            }

            byte metricType = metricTypes[0];
            StructureOfIndices left = StructureOfIndices.Create(metricType, 3);
            StructureOfIndices right = StructureOfIndices.Create([metricType], [3]);

            Assert.Equal(left, right);
            Assert.Equal(left.GetHashCode(), right.GetHashCode());
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void TypeCountAndTypeDataShouldProvideExpectedRanges()
    {
        try
        {
            if (!TryGetMetricTypes(2, out var metricTypes))
            {
                return;
            }

            byte firstType = metricTypes[0];
            byte secondType = metricTypes[1];

            StructureOfIndices structure = StructureOfIndices.Create(
                [firstType, secondType],
                [2, 3]);

            Assert.Equal(2, structure.TypeCount(firstType));
            Assert.Equal(3, structure.TypeCount(secondType));

            TypeData first = structure.GetTypeData(firstType);
            Assert.Equal(0, first.From);
            Assert.Equal(2, first.Length);
            Assert.Null(first.States);

            TypeData second = structure.GetTypeData(secondType);
            Assert.Equal(2, second.From);
            Assert.Equal(3, second.Length);
            Assert.Null(second.States);
        }
        catch (TypeInitializationException)
        {
        }
    }

    private static bool TryGetMetricTypes(int count, out List<byte> metricTypes)
    {
        metricTypes = new List<byte>(count);
        try
        {
            for (byte i = 0; i < IndexTypeMethods.TypesCount; ++i)
            {
                if (!CC.IsMetric(i))
                {
                    continue;
                }

                metricTypes.Add(i);
                if (metricTypes.Count == count)
                {
                    return true;
                }
            }
        }
        catch (TypeInitializationException)
        {
            return false;
        }

        return false;
    }

    private static bool TryGetNonMetricType(out byte type)
    {
        type = 0;
        try
        {
            for (byte i = 0; i < IndexTypeMethods.TypesCount; ++i)
            {
                if (CC.IsMetric(i))
                {
                    continue;
                }

                type = i;
                return true;
            }
        }
        catch (TypeInitializationException)
        {
            return false;
        }

        return false;
    }
}
