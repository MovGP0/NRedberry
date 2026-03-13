using NRedberry.Indices;
using NRedberry.Parsers;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public sealed class StructureOfIndicesTest
{
    [Fact]
    public void ShouldDetectStructure()
    {
        SimpleIndices indices = ParserIndices.ParseSimple("_ab'c'^d'");
        StructureOfIndices structure = StructureOfIndices.Create(indices);

        Assert.Equal(indices.StructureOfIndices.ToString(), structure.ToString());
        Assert.NotEqual(ParserIndices.ParseSimple("_ab'^c'd'").StructureOfIndices.ToString(), structure.ToString());
        Assert.Equal(ParserIndices.ParseSimple("^a_b'c'^d'").StructureOfIndices.ToString(), structure.ToString());
    }

    [Fact]
    public void ShouldCompareDifferentNames()
    {
        Assert.NotEqual(TensorApi.Parse("v_a'").GetHashCode(), TensorApi.Parse("v^a'").GetHashCode());
        Assert.NotEqual(TensorApi.Parse("v_a").GetHashCode(), TensorApi.Parse("v^a").GetHashCode());
    }

    [Fact]
    public void ShouldInvertStructure()
    {
        SimpleIndices indices = ParserIndices.ParseSimple("_ab'c'^d'");
        NRedberry.Indices.Indices inverted = indices.GetInverted();

        Assert.Equal(indices.StructureOfIndices.GetInverted().ToString(), ((SimpleIndices)inverted).StructureOfIndices.ToString());
    }

    [Fact]
    public void ShouldAppendStructures()
    {
        SimpleIndices left = ParserIndices.ParseSimple("_ab'c'^d'_g'");
        SimpleIndices other = ParserIndices.ParseSimple("_ab_\\alpha_b'c'^d'_g'");
        SimpleIndices leftThenOther = ParserIndices.ParseSimple("_ab'c'^d'_g'_xy_\\beta_y't'^w'_q'");
        SimpleIndices otherThenLeft = ParserIndices.ParseSimple("_xy_\\beta_y't'^w'_q'_ab'c'^d'_g'");

        Assert.Equal(leftThenOther.StructureOfIndices.ToString(), left.StructureOfIndices.Append(other.StructureOfIndices).ToString());
        Assert.Equal(otherThenLeft.StructureOfIndices.ToString(), other.StructureOfIndices.Append(left.StructureOfIndices).ToString());
        Assert.Equal(
            ((SimpleIndices)leftThenOther.GetInverted()).StructureOfIndices.ToString(),
            ((SimpleIndices)left.GetInverted()).StructureOfIndices.Append(((SimpleIndices)other.GetInverted()).StructureOfIndices).ToString());
        Assert.Equal(
            ((SimpleIndices)otherThenLeft.GetInverted()).StructureOfIndices.ToString(),
            ((SimpleIndices)other.GetInverted()).StructureOfIndices.Append(((SimpleIndices)left.GetInverted()).StructureOfIndices).ToString());
        Assert.Equal(
            ((SimpleIndices)leftThenOther.GetInverted()).StructureOfIndices.ToString(),
            left.StructureOfIndices.GetInverted().Append(other.StructureOfIndices.GetInverted()).ToString());
        Assert.Equal(
            ((SimpleIndices)otherThenLeft.GetInverted()).StructureOfIndices.ToString(),
            other.StructureOfIndices.GetInverted().Append(left.StructureOfIndices.GetInverted()).ToString());
    }

    [Fact]
    public void ShouldSubtractStructures()
    {
        SimpleIndices left = ParserIndices.ParseSimple("_ab'c'^d'_g'");
        SimpleIndices other = ParserIndices.ParseSimple("_ab_\\alpha_b'c'^d'_g'");
        SimpleIndices leftThenOther = ParserIndices.ParseSimple("_ab'c'^d'_g'_xy_\\beta_y't'^w'_q'");
        SimpleIndices otherThenLeft = ParserIndices.ParseSimple("_xy_\\beta_y't'^w'_q'_ab'c'^d'_g'");

        Assert.Equal(left.StructureOfIndices.ToString(), leftThenOther.StructureOfIndices.Subtract(other.StructureOfIndices).ToString());
        Assert.Equal(other.StructureOfIndices.ToString(), otherThenLeft.StructureOfIndices.Subtract(left.StructureOfIndices).ToString());
        Assert.Equal(
            left.StructureOfIndices.GetInverted().ToString(),
            leftThenOther.StructureOfIndices.GetInverted().Subtract(other.StructureOfIndices.GetInverted()).ToString());
        Assert.Equal(
            other.StructureOfIndices.GetInverted().ToString(),
            otherThenLeft.StructureOfIndices.GetInverted().Subtract(left.StructureOfIndices.GetInverted()).ToString());
    }

    [Fact]
    public void ShouldThrowOnInvalidSubtract()
    {
        SimpleIndices other = ParserIndices.ParseSimple("_ab_\\alpha_b'c'^d'_g'");
        SimpleIndices invalid = ParserIndices.ParseSimple("_ab'c'^d'_g'_xy_\\beta_y't'w'q'");

        Action action = () => invalid.StructureOfIndices.Subtract(other.StructureOfIndices);

        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void ShouldPartitionStructures()
    {
        SimpleIndices left = ParserIndices.ParseSimple("_ab'c'^d'_g'");
        SimpleIndices other = ParserIndices.ParseSimple("_ab_\\alpha_b'c'^d'_g'");
        SimpleIndices combined = ParserIndices.ParseSimple("_ab'c'^d'_g'_xy_\\beta_y't'^w'_q'");

        StructureOfIndices[] partition = [left.StructureOfIndices, other.StructureOfIndices];
        int[][] mappings = combined.StructureOfIndices.GetPartitionMappings(partition);
        int partitionIndex = 0;

        foreach (int[] mapping in mappings)
        {
            List<int> rebuilt = [];
            foreach (int index in mapping)
            {
                rebuilt.Add(combined[index]);
            }

            Assert.Equal(
                partition[partitionIndex++].ToString(),
                IndicesFactory.CreateSimple(null, [.. rebuilt]).StructureOfIndices.ToString());
        }
    }
}
