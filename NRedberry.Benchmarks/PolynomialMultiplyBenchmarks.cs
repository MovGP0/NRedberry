using BenchmarkDotNet.Attributes;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

namespace NRedberry.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class PolynomialMultiplyBenchmarks
{
    private GenPolynomial<ModLong> _polynomial = null!;
    private ModLong _coefficient = null!;
    private ExpVector _shift = null!;

    [GlobalSetup]
    public void Setup()
    {
        var coefficientRing = new ModLongRing(2_147_483_647L, true);
        var polynomialRing = new GenPolynomialRing<ModLong>(coefficientRing, 1, ["x"]);
        _polynomial = polynomialRing.Zero;

        for (int exponent = 0; exponent < 512; exponent++)
        {
            ModLong coefficient = coefficientRing.FromInteger((exponent % 97) + 1L);
            ExpVector monomialExponent = ExpVector.Create(1, 0, exponent);
            _polynomial = _polynomial.Sum(new GenPolynomial<ModLong>(polynomialRing, coefficient, monomialExponent));
        }

        _coefficient = coefficientRing.FromInteger(37L);
        _shift = ExpVector.Create(1, 0, 9L);
    }

    [Benchmark(OperationsPerInvoke = 1_000)]
    public GenPolynomial<ModLong> GenPolynomialMultiplyByMonomial()
    {
        GenPolynomial<ModLong> result = _polynomial;
        for (int i = 0; i < 1_000; i++)
        {
            result = result.Multiply(_coefficient, _shift);
        }

        return result;
    }
}
