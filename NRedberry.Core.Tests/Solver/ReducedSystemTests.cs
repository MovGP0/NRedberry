using System.Runtime.CompilerServices;
using NRedberry.Solver;
using NRedberry.Tensors;

namespace NRedberry.Core.Tests.Solver;

public sealed class ReducedSystemTests
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenEquationsAreNull()
    {
        SimpleTensor[] unknownCoefficients = [];
        Expression[] generalSolutions = [];

        Should.Throw<ArgumentNullException>(() => new ReducedSystem(null!, unknownCoefficients, generalSolutions));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenUnknownCoefficientsAreNull()
    {
        Expression[] equations = [];
        Expression[] generalSolutions = [];

        Should.Throw<ArgumentNullException>(() => new ReducedSystem(equations, null!, generalSolutions));
    }

    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenGeneralSolutionsAreNull()
    {
        Expression[] equations = [];
        SimpleTensor[] unknownCoefficients = [];

        Should.Throw<ArgumentNullException>(() => new ReducedSystem(equations, unknownCoefficients, null!));
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

        equationsSnapshot.ShouldNotBeSameAs(equations);
        unknownCoefficientsSnapshot.ShouldNotBeSameAs(unknownCoefficients);
        generalSolutionsSnapshot.ShouldNotBeSameAs(generalSolutions);
        equationsSnapshot.ShouldHaveSingleItem();
        unknownCoefficientsSnapshot.ShouldHaveSingleItem();
        generalSolutionsSnapshot.ShouldHaveSingleItem();

        equationsSnapshot[0] = replacementExpression;
        unknownCoefficientsSnapshot[0] = replacementSimpleTensor;
        generalSolutionsSnapshot[0] = replacementExpression;

        reducedSystem.GetEquations()[0].ShouldBeSameAs(expression);
        reducedSystem.GetUnknownCoefficients()[0].ShouldBeSameAs(simpleTensor);
        reducedSystem.GetGeneralSolutions()[0].ShouldBeSameAs(expression);
    }
}
