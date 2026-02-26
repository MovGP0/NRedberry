using System.Runtime.CompilerServices;
using NRedberry.Solver;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Solver;

public sealed class ReducedSystemTests
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenEquationsAreNull()
    {
        SimpleTensor[] unknownCoefficients = [];
        Expression[] generalSolutions = [];

        Assert.Throws<ArgumentNullException>(() => new ReducedSystem(null!, unknownCoefficients, generalSolutions));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenUnknownCoefficientsAreNull()
    {
        Expression[] equations = [];
        Expression[] generalSolutions = [];

        Assert.Throws<ArgumentNullException>(() => new ReducedSystem(equations, null!, generalSolutions));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenGeneralSolutionsAreNull()
    {
        Expression[] equations = [];
        SimpleTensor[] unknownCoefficients = [];

        Assert.Throws<ArgumentNullException>(() => new ReducedSystem(equations, unknownCoefficients, null!));
    }

    [Fact]
    public void ShouldReturnClonedArraysFromGetters()
    {
        Expression expression = (Expression)RuntimeHelpers.GetUninitializedObject(typeof(Expression));
        Expression replacementExpression = (Expression)RuntimeHelpers.GetUninitializedObject(typeof(Expression));
        SimpleTensor simpleTensor = (SimpleTensor)RuntimeHelpers.GetUninitializedObject(typeof(SimpleTensor));
        SimpleTensor replacementSimpleTensor = (SimpleTensor)RuntimeHelpers.GetUninitializedObject(typeof(SimpleTensor));
        Expression[] equations = [expression];
        SimpleTensor[] unknownCoefficients = [simpleTensor];
        Expression[] generalSolutions = [expression];
        ReducedSystem reducedSystem = new(equations, unknownCoefficients, generalSolutions);

        Expression[] equationsSnapshot = reducedSystem.GetEquations();
        SimpleTensor[] unknownCoefficientsSnapshot = reducedSystem.GetUnknownCoefficients();
        Expression[] generalSolutionsSnapshot = reducedSystem.GetGeneralSolutions();

        Assert.NotSame(equations, equationsSnapshot);
        Assert.NotSame(unknownCoefficients, unknownCoefficientsSnapshot);
        Assert.NotSame(generalSolutions, generalSolutionsSnapshot);
        Assert.Single(equationsSnapshot);
        Assert.Single(unknownCoefficientsSnapshot);
        Assert.Single(generalSolutionsSnapshot);

        equationsSnapshot[0] = replacementExpression;
        unknownCoefficientsSnapshot[0] = replacementSimpleTensor;
        generalSolutionsSnapshot[0] = replacementExpression;

        Assert.Same(expression, reducedSystem.GetEquations()[0]);
        Assert.Same(simpleTensor, reducedSystem.GetUnknownCoefficients()[0]);
        Assert.Same(expression, reducedSystem.GetGeneralSolutions()[0]);
    }
}
