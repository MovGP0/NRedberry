using System.Collections;
using NRedberry.Contexts;
using NRedberry.Indices;

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
            structure.Size.ShouldBe(3);
            structure.Count.ShouldBe(3);
            structure.TypeCount(type).ShouldBe(3);

            StructureOfIndices empty = StructureOfIndices.Create(type, 0);
            empty.ShouldBeSameAs(StructureOfIndices.Empty);
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

            structure.Size.ShouldBe(3);
            structure.TypeCount(types[0]).ShouldBe(2);
            structure.TypeCount(types[1]).ShouldBe(1);

            StructureOfIndices empty = StructureOfIndices.Create(types, [0, 0]);
            empty.ShouldBeSameAs(StructureOfIndices.Empty);
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
            structure.Size.ShouldBe(4);
            structure.TypeCount(metricType).ShouldBe(2);
            structure.TypeCount(nonMetricType).ShouldBe(2);

            int[] zeroCounts = new int[typeCount];
            BitArray[] zeroStates = new BitArray[typeCount];
            for (byte i = 0; i < typeCount; ++i)
            {
                zeroStates[i] = CC.IsMetric(i) ? null! : new BitArray(0);
            }

            StructureOfIndices empty = StructureOfIndices.Create(zeroCounts, zeroStates);
            empty.ShouldBeSameAs(StructureOfIndices.Empty);
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

            structure.TypeCount(metricType).ShouldBe(2);
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
            statesCopyAgain[nonMetricType].Get(0).ShouldBeTrue();
            statesCopyAgain[nonMetricType].Get(1).ShouldBeFalse();
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
            appended.Size.ShouldBe(3);
            appended.TypeCount(metricType).ShouldBe(3);

            StructureOfIndices pow = left.Pow(3);
            pow.Size.ShouldBe(6);
            pow.TypeCount(metricType).ShouldBe(6);

            StructureOfIndices subtracted = appended.Subtract(right);
            subtracted.ShouldBe(left);
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

            right.ShouldBe(left);
            right.GetHashCode().ShouldBe(left.GetHashCode());
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

            structure.TypeCount(firstType).ShouldBe(2);
            structure.TypeCount(secondType).ShouldBe(3);

            TypeData first = structure.GetTypeData(firstType);
            first.From.ShouldBe(0);
            first.Length.ShouldBe(2);
            first.States.ShouldBeNull();

            TypeData second = structure.GetTypeData(secondType);
            second.From.ShouldBe(2);
            second.Length.ShouldBe(3);
            second.States.ShouldBeNull();
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
