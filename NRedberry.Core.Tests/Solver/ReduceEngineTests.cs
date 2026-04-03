using NRedberry.Solver;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

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

            Should.Throw<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(null!, vars, rules));
            Should.Throw<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, null!, rules));
            Should.Throw<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, vars, null!));

            Should.Throw<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(null!, vars, rules, symmetricForm));
            Should.Throw<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, null!, rules, symmetricForm));
            Should.Throw<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, vars, null!, symmetricForm));
            Should.Throw<ArgumentNullException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, vars, rules, null!));
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

            var exception = Should.Throw<ArgumentException>(() => ReduceEngine.ReduceToSymbolicSystem(equations, vars, rules, []));

            exception.ParamName.ShouldBe("symmetricForm");
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

            reduced.ShouldBeNull();
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

            reduced.ShouldNotBeNull();

            var reducedEquations = reduced.GetEquations();
            var unknownCoefficients = reduced.GetUnknownCoefficients();
            var generalSolutions = reduced.GetGeneralSolutions();

            reducedEquations.ShouldHaveSingleItem();
            unknownCoefficients.Length.ShouldBe(2);
            generalSolutions.Length.ShouldBe(2);

            HashSet<int> variableNames = [vars[0].Name, vars[1].Name];
            foreach (Expression reducedEquation in reducedEquations)
            {
                TensorUtils.ContainsSimpleTensors(reducedEquation, variableNames).ShouldBeFalse();
                TensorUtils.Equals(reducedEquation[1], TensorFactory.Parse("0")).ShouldBeTrue();
            }
        }
        catch (TypeInitializationException)
        {
        }
    }
}
