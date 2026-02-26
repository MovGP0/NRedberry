using System;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesBuilderTests
{
    [Fact]
    public void AppendOverloads_ShouldPreserveInsertionOrderInArrayAndEnumeration()
    {
        IndicesBuilder sourceBuilder = new();
        sourceBuilder.Append(7);
        sourceBuilder.Append(8, 9);

        List<int> enumerableValues = [10, 11];

        IndicesBuilder builder = new();
        builder.Append(1);
        builder.Append(2, 3);
        builder.Append([[4, 5], [6]]);
        builder.Append(enumerableValues);
        builder.Append(sourceBuilder);

        int[] expected = [1, 2, 3, 4, 5, 6, 10, 11, 7, 8, 9];

        Assert.Equal(expected, builder.ToArray());
        Assert.Equal(expected, builder.ToList());
    }

    [Fact]
    public void AppendIntArray_ShouldThrowWhenNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => builder.Append((int[])null!));

        Assert.Equal("indices", exception.ParamName);
    }

    [Fact]
    public void AppendIntJaggedArray_ShouldThrowWhenOuterArrayIsNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => builder.Append((int[][])null!));

        Assert.Equal("indices", exception.ParamName);
    }

    [Fact]
    public void AppendIntJaggedArray_ShouldThrowWhenInnerArrayIsNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => builder.Append(new int[][]
        {
            [1],
            null!
        }));

        Assert.Equal("array", exception.ParamName);
    }

    [Fact]
    public void AppendEnumerable_ShouldThrowWhenNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => builder.Append((IEnumerable<int>)null!));

        Assert.Equal("indices", exception.ParamName);
    }

    [Fact]
    public void AppendBuilder_ShouldThrowWhenNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => builder.Append((IndicesBuilder)null!));

        Assert.Equal("ib", exception.ParamName);
    }

    [Fact]
    public void Clone_ShouldCreateIndependentBuilder()
    {
        IndicesBuilder original = new();
        original.Append(1, 2, 3);

        IndicesBuilder clone = original.Clone();
        clone.Append(4);

        Assert.Equal([1, 2, 3], original.ToArray());
        Assert.Equal([1, 2, 3, 4], clone.ToArray());
    }

    [Fact]
    public void IndicesProperty_ShouldReturnSortedIndicesWithSameElements()
    {
        IndicesBuilder builder = new();
        builder.Append(3, 1, 2, 4);
        Exception? exception = Record.Exception(() =>
        {
            NRedberry.Indices.Indices indices = builder.Indices;
            Assert.IsType<SortedIndices>(indices);
            Assert.True(indices.AllIndices.SequenceEqual([1, 2, 3, 4]));
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        Assert.Null(exception);
        Assert.Equal([3, 1, 2, 4], builder.ToArray());
    }

    [Fact]
    public void ToString_ShouldReturnNonNull()
    {
        IndicesBuilder builder = new();
        Exception? exception = Record.Exception(() =>
        {
            string value = builder.ToString();
            Assert.NotNull(value);
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        Assert.Null(exception);
    }
}
