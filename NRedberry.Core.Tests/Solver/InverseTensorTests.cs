using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Solver;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Solver;

public sealed class InverseTensorTests
{
    [Fact]
    public void ShouldThrowWhenConstructorArgumentsAreNull()
    {
        Expression expression = CreateExpression(CreateSimpleTensor(1), CreateSimpleTensor(2));
        TensorType[] emptySamples = [];

        Assert.Throws<ArgumentNullException>(() => new InverseTensor(null!, expression, emptySamples));
        Assert.Throws<ArgumentNullException>(() => new InverseTensor(expression, null!, emptySamples));
        Assert.Throws<ArgumentNullException>(() => new InverseTensor(expression, expression, null!));
        Assert.Throws<ArgumentNullException>(() => new InverseTensor(expression, expression, emptySamples, false, null!));
    }

    [Fact]
    public void ShouldThrowWhenEquationLeftHandSideIsNotProduct()
    {
        Expression toInverse = CreateExpression(CreateSimpleTensor(1), CreateSimpleTensor(2));
        Expression equation = CreateExpression(CreateSimpleTensor(3), CreateSimpleTensor(4));
        TensorType[] samples = [];

        var exception = Assert.Throws<ArgumentException>(() => new InverseTensor(toInverse, equation, samples));
        Assert.Equal("equation", exception.ParamName);
    }

    private static SimpleTensor CreateSimpleTensor(int name)
    {
        return new SimpleTensor(name, IndicesFactory.EmptySimpleIndices);
    }

    private static Expression CreateExpression(TensorType left, TensorType right)
    {
        return new Expression(IndicesFactory.EmptyIndices, left, right);
    }
}
