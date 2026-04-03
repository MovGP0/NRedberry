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

        structure.ToString().ShouldBe(indices.StructureOfIndices.ToString());
        structure.ToString().ShouldNotBe(ParserIndices.ParseSimple("_ab'^c'd'").StructureOfIndices.ToString());
        structure.ToString().ShouldBe(ParserIndices.ParseSimple("^a_b'c'^d'").StructureOfIndices.ToString());
    }

    [Fact]
    public void ShouldCompareDifferentNames()
    {
        TensorApi.Parse("v^a'").GetHashCode().ShouldNotBe(TensorApi.Parse("v_a'").GetHashCode());
        TensorApi.Parse("v^a").GetHashCode().ShouldBe(TensorApi.Parse("v_a").GetHashCode());
    }

    [Fact]
    public void ShouldInvertStructure()
    {
        SimpleIndices indices = ParserIndices.ParseSimple("_ab'c'^d'");
        NRedberry.Indices.Indices inverted = indices.GetInverted();

        ((SimpleIndices)inverted).StructureOfIndices.ToString().ShouldBe(indices.StructureOfIndices.GetInverted().ToString());
    }

    [Fact]
    public void ShouldAppendStructures()
    {
        SimpleIndices left = ParserIndices.ParseSimple("_ab'c'^d'_g'");
        SimpleIndices other = ParserIndices.ParseSimple("_ab_\\alpha_b'c'^d'_g'");
        SimpleIndices leftThenOther = ParserIndices.ParseSimple("_ab'c'^d'_g'_xy_\\beta_y't'^w'_q'");
        SimpleIndices otherThenLeft = ParserIndices.ParseSimple("_xy_\\beta_y't'^w'_q'_ab'c'^d'_g'");

        left.StructureOfIndices.Append(other.StructureOfIndices).ToString().ShouldBe(leftThenOther.StructureOfIndices.ToString());
        other.StructureOfIndices.Append(left.StructureOfIndices).ToString().ShouldBe(otherThenLeft.StructureOfIndices.ToString());
        ((SimpleIndices)left.GetInverted()).StructureOfIndices.Append(((SimpleIndices)other.GetInverted()).StructureOfIndices).ToString().ShouldBe(((SimpleIndices)leftThenOther.GetInverted()).StructureOfIndices.ToString());
        ((SimpleIndices)other.GetInverted()).StructureOfIndices.Append(((SimpleIndices)left.GetInverted()).StructureOfIndices).ToString().ShouldBe(((SimpleIndices)otherThenLeft.GetInverted()).StructureOfIndices.ToString());
        left.StructureOfIndices.GetInverted().Append(other.StructureOfIndices.GetInverted()).ToString().ShouldBe(((SimpleIndices)leftThenOther.GetInverted()).StructureOfIndices.ToString());
        other.StructureOfIndices.GetInverted().Append(left.StructureOfIndices.GetInverted()).ToString().ShouldBe(((SimpleIndices)otherThenLeft.GetInverted()).StructureOfIndices.ToString());
    }

    [Fact]
    public void ShouldSubtractStructures()
    {
        SimpleIndices left = ParserIndices.ParseSimple("_ab'c'^d'_g'");
        SimpleIndices other = ParserIndices.ParseSimple("_ab_\\alpha_b'c'^d'_g'");
        SimpleIndices leftThenOther = ParserIndices.ParseSimple("_ab'c'^d'_g'_xy_\\beta_y't'^w'_q'");
        SimpleIndices otherThenLeft = ParserIndices.ParseSimple("_xy_\\beta_y't'^w'_q'_ab'c'^d'_g'");

        leftThenOther.StructureOfIndices.Subtract(other.StructureOfIndices).ToString().ShouldBe(left.StructureOfIndices.ToString());
        otherThenLeft.StructureOfIndices.Subtract(left.StructureOfIndices).ToString().ShouldBe(other.StructureOfIndices.ToString());
        leftThenOther.StructureOfIndices.GetInverted().Subtract(other.StructureOfIndices.GetInverted()).ToString().ShouldBe(left.StructureOfIndices.GetInverted().ToString());
        otherThenLeft.StructureOfIndices.GetInverted().Subtract(left.StructureOfIndices.GetInverted()).ToString().ShouldBe(other.StructureOfIndices.GetInverted().ToString());
    }

    [Fact]
    public void ShouldThrowOnInvalidSubtract()
    {
        SimpleIndices other = ParserIndices.ParseSimple("_ab_\\alpha_b'c'^d'_g'");
        SimpleIndices invalid = ParserIndices.ParseSimple("_ab'c'^d'_g'_xy_\\beta_y't'w'q'");

        Action action = () => invalid.StructureOfIndices.Subtract(other.StructureOfIndices);

        Should.Throw<ArgumentException>(action);
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

            IndicesFactory.CreateSimple(null, [.. rebuilt]).StructureOfIndices.ToString().ShouldBe(partition[partitionIndex++].ToString());
        }
    }
}
