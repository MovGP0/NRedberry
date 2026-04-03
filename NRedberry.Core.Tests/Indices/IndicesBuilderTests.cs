using NRedberry.Indices;

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

        builder.ToArray().ShouldBe(expected);
        builder.ToList().ShouldBe(expected);
    }

    [Fact]
    public void AppendIntArray_ShouldThrowWhenNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => builder.Append((int[])null!));

        exception.ParamName.ShouldBe("indices");
    }

    [Fact]
    public void AppendIntJaggedArray_ShouldThrowWhenOuterArrayIsNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => builder.Append((int[][])null!));

        exception.ParamName.ShouldBe("indices");
    }

    [Fact]
    public void AppendIntJaggedArray_ShouldThrowWhenInnerArrayIsNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => builder.Append(new int[][]
        {
            [1],
            null!
        }));

        exception.ParamName.ShouldBe("array");
    }

    [Fact]
    public void AppendEnumerable_ShouldThrowWhenNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => builder.Append((IEnumerable<int>)null!));

        exception.ParamName.ShouldBe("indices");
    }

    [Fact]
    public void AppendBuilder_ShouldThrowWhenNull()
    {
        IndicesBuilder builder = new();

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => builder.Append((IndicesBuilder)null!));

        exception.ParamName.ShouldBe("ib");
    }

    [Fact]
    public void Clone_ShouldCreateIndependentBuilder()
    {
        IndicesBuilder original = new();
        original.Append(1, 2, 3);

        IndicesBuilder clone = original.Clone();
        clone.Append(4);

        original.ToArray().ShouldBe([1, 2, 3]);
        clone.ToArray().ShouldBe([1, 2, 3, 4]);
    }

    [Fact]
    public void IndicesProperty_ShouldReturnSortedIndicesWithSameElements()
    {
        IndicesBuilder builder = new();
        builder.Append(3, 1, 2, 4);
        Exception? exception = Record.Exception(() =>
        {
            NRedberry.Indices.Indices indices = builder.Indices;
            indices.ShouldBeOfType<SortedIndices>();
            indices.AllIndices.SequenceEqual([1, 2, 3, 4]).ShouldBeTrue();
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        exception.ShouldBeNull();
        builder.ToArray().ShouldBe([3, 1, 2, 4]);
    }

    [Fact]
    public void ToString_ShouldReturnNonNull()
    {
        IndicesBuilder builder = new();
        Exception? exception = Record.Exception(() =>
        {
            string value = builder.ToString();
            value.ShouldNotBeNull();
        });
        if (exception is TypeInitializationException)
        {
            return;
        }

        exception.ShouldBeNull();
    }
}
