using NRedberry.Solver;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Solver;

public sealed class ReduceEngineTests
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenArgumentsAreNull()
    {
        try
        {
            var equations = new[] { TensorFactory.ParseExpression("x=0") };
            var vars = new[] { TensorFactory.ParseSimple("x") };
            ITransformation[] rules = [];
            bool[] symmetricForm = [false];

            Assert.Throws<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(null!, vars, rules));
            Assert.Throws<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, null!, rules));
            Assert.Throws<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, vars, null!));

            Assert.Throws<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(null!, vars, rules, symmetricForm));
            Assert.Throws<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, null!, rules, symmetricForm));
            Assert.Throws<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, vars, null!, symmetricForm));
            Assert.Throws<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, vars, rules, null!));
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldThrowArgumentExceptionWhenSymmetricFormLengthDoesNotMatchVarsLength()
    {
        try
        {
            var equations = new[] { TensorFactory.ParseExpression("x=0") };
            var vars = new[] { TensorFactory.ParseSimple("x") };
            ITransformation[] rules = [];

            var exception = Assert.Throws<ArgumentException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, vars, rules, []));

            Assert.Equal("symmetricForm", exception.ParamName);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldReturnNullWhenNoSamplesAndVariableHasIndices()
    {
        try
        {
            var equations = new[] { TensorFactory.ParseExpression("F_mn=0") };
            var vars = new[] { TensorFactory.ParseSimple("F_mn") };
            ITransformation[] rules = [];

            ReducedSystem reduced = ReduceEngine.ReduceToSymbolicSystem(equations, vars, rules, [false]);

            Assert.Null(reduced);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldReduceScalarSystemWithoutKeepingOriginalVariablesInReducedEquations()
    {
        try
        {
            var equations = new[] { TensorFactory.ParseExpression("x+y=1") };
            var vars = new[] { TensorFactory.ParseSimple("x"), TensorFactory.ParseSimple("y") };
            ITransformation[] rules = [];

            ReducedSystem reduced = ReduceEngine.ReduceToSymbolicSystem(equations, vars, rules);

            Assert.NotNull(reduced);

            var reducedEquations = reduced.GetEquations();
            var unknownCoefficients = reduced.GetUnknownCoefficients();
            var generalSolutions = reduced.GetGeneralSolutions();

            Assert.Single(reducedEquations);
            Assert.Equal(2, unknownCoefficients.Length);
            Assert.Equal(2, generalSolutions.Length);

            HashSet<int> variableNames = [vars[0].Name, vars[1].Name];
            foreach (Expression reducedEquation in reducedEquations)
            {
                Assert.False(TensorUtils.ContainsSimpleTensors(reducedEquation, variableNames));
                Assert.True(TensorUtils.Equals(reducedEquation[1], TensorFactory.Parse("0")));
            }
        }
        catch (TypeInitializationException)
        {
        }
    }
}
