using BenchmarkDotNet.Attributes;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using NRedberry.Tensors;
using NRedberry.Transformations.Factor;

namespace NRedberry.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class SquarefreeEngineBenchmarks
{
    private const string ResidualFormula =
        "2304*m**8 - 1152*s*m**6 + 288*m**4*s**2 - 1536*m**6*t + 480*m**4*s*t - 48*m**2*s**2*t + 352*m**4*t**2 - 56*m**2*s*t**2 + 2*s**2*t**2 - 32*m**2*t**3 + 2*s*t**3 + t**4";

    private GenPolynomial<BigInteger> IntegerPolynomial { get; set; } = null!;

    private SquarefreeAbstract<BigInteger> SquarefreeEngine { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        Tensor test1Residual = Tensors.Tensors.Parse(ResidualFormula);
        IntegerPolynomial = JasFactor.TensorToPoly(test1Residual);
        SquarefreeEngine = SquarefreeFactory.GetImplementation(BigInteger.One);
    }

    [Benchmark]
    public SortedDictionary<GenPolynomial<BigInteger>, long> SquarefreeFactorsResidualPolynomial()
    {
        return SquarefreeEngine.SquarefreeFactors(IntegerPolynomial);
    }
}
