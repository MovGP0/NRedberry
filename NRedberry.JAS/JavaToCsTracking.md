# Java to C# Tracking

This document tracks the status of all files in the NRedberry.JAS project.

## Files

### Arith (12 files)

| Java File                          | C# File                          | Status | Notes |
|------------------------------------|----------------------------------|--------|-------|
| BigComplex.java                    | BigComplex.cs                    |        |       |
| BigInteger.java                    | BigInteger.cs                    |        |       |
| BigRational.java                   | BigRational.cs                   |        |       |
| Combinatoric.java                  | Combinatoric.cs                  |        |       |
| ModInteger.java                    | ModInteger.cs                    |        |       |
| ModIntegerRing.java                | ModIntegerRing.cs                |        |       |
| ModLong.java                       | ModLong.cs                       |        |       |
| ModLongRing.java                   | ModLongRing.cs                   |        |       |
| Modular.java                       | Modular.cs                       |        |       |
| ModularNotInvertibleException.java | ModularNotInvertibleException.cs |        |       |
| ModularRingFactory.java            | ModularRingFactory.cs            |        |       |
| PrimeList.java                     | PrimeList.cs                     |        |       |

### Gb (4 files)

| Java File              | C# File              | Status | Notes |
|------------------------|----------------------|--------|-------|
| GroebnerBase.java      | GroebnerBase.cs      |        |       |
| Reduction.java         | Reduction.cs         |        |       |
| ReductionAbstract.java | ReductionAbstract.cs |        |       |
| ReductionSeq.java      | ReductionSeq.cs      |        |       |

### Poly (24 files)

| Java File                            | C# File                      | Status | Notes |
|--------------------------------------|------------------------------|--------|-------|
| AlgebraicNotInvertibleException.java |                              |        |       |
| AlgebraicNumber.java                 | AlgebraicNumber.cs           |        |       |
| AlgebraicNumberRing.java             | AlgebraicNumberRing.cs       |        |       |
| Complex.java                         | Complex.cs                   |        |       |
| ComplexRing.java                     | ComplexRing.cs               |        |       |
| ExpVector.java                       | ExpVector.cs                 |        |       |
| ExpVectorByte.java                   | ExpVectorByte.cs             |        |       |
| ExpVectorInteger.java                | ExpVectorInteger.cs          |        |       |
| ExpVectorLong.java                   | ExpVectorLong.cs             |        |       |
| ExpVectorPair.java                   | ExpVectorPair.cs             |        |       |
| ExpVectorShort.java                  | ExpVectorShort.cs            |        |       |
| GenPolynomial.java                   | GenPolynomial.cs             |        |       |
| GenPolynomialRing.java               | GenPolynomialRing.cs         |        |       |
| GenSolvablePolynomial.java           | GenSolvablePolynomial.cs     |        |       |
| GenSolvablePolynomialRing.java       | GenSolvablePolynomialRing.cs |        |       |
| Monomial.java                        | Monomial.cs                  |        |       |
| OptimizedPolynomialList.java         | OptimizedPolynomialList.cs   |        |       |
| OrderedPolynomialList.java           | OrderedPolynomialList.cs     |        |       |
| PolyIterator.java                    | PolyIterator.cs              |        |       |
| PolynomialComparator.java            | PolynomialComparator.cs      |        |       |
| PolynomialList.java                  | PolynomialList.cs            |        |       |
| PolyUtil.java                        | PolyUtil.cs                  |        |       |
| RelationTable.java                   | RelationTable.cs             |        |       |
| TermOrder.java                       | TermOrder.cs                 |        |       |
| TermOrderOptimization.java           | TermOrderOptimization.cs     |        |       |

### Ps (5 files)

| Java File                     | C# File                     | Status | Notes |
|-------------------------------|-----------------------------|--------|-------|
| Coefficients.java             | Coefficients.cs             |        |       |
| PolynomialTaylorFunction.java | PolynomialTaylorFunction.cs |        |       |
| TaylorFunction.java           | TaylorFunction.cs           |        |       |
| UnivPowerSeries.java          | UnivPowerSeries.cs          |        |       |
| UnivPowerSeriesRing.java      | UnivPowerSeriesRing.cs      |        |       |

### Structure (13 files)

| Java File                   | C# File                   | Status | Notes |
|-----------------------------|---------------------------|--------|-------|
| AbelianGroupElem.java       | AbelianGroupElem.cs       |        |       |
| AbelianGroupFactory.java    | AbelianGroupFactory.cs    |        |       |
| BinaryFunctor.java          | BinaryFunctor.cs          |        |       |
| Element.java                | Element.cs                |        |       |
| ElemFactory.java            | ElemFactory.cs            |        |       |
| GcdRingElem.java            | GcdRingElem.cs            |        |       |
| MonoidElem.java             | MonoidElem.cs             |        |       |
| MonoidFactory.java          | MonoidFactory.cs          |        |       |
| NotInvertibleException.java | NotInvertibleException.cs |        |       |
| Power.java                  | Power.cs                  |        |       |
| RingElem.java               | RingElem.cs               |        |       |
| RingFactory.java            | RingFactory.cs            |        |       |
| UnaryFunctor.java           | UnaryFunctor.cs           |        |       |

### Ufd (37 files)

| Java File                                  | C# File                                  | Status | Notes |
|--------------------------------------------|------------------------------------------|--------|-------|
| FactorAbsolute.java                        | FactorAbsolute.cs                        |        |       |
| FactorAbstract.java                        | FactorAbstract.cs                        |        |       |
| FactorAlgebraic.java                       | FactorAlgebraic.cs                       |        |       |
| FactorComplex.java                         | FactorComplex.cs                         |        |       |
| FactorFactory.java                         | FactorFactory.cs                         |        |       |
| FactorInteger.java                         | FactorInteger.cs                         |        |       |
| Factorization.java                         | Factorization.cs                         |        |       |
| FactorModular.java                         | FactorModular.cs                         |        |       |
| FactorQuotient.java                        | FactorQuotient.cs                        |        |       |
| FactorRational.java                        | FactorRational.cs                        |        |       |
| Factors.java                               | Factors.cs                               |        |       |
| GCDFactory.java                            | GCDFactory.cs                            |        |       |
| GreatestCommonDivisor.java                 | GreatestCommonDivisor.cs                 |        |       |
| GreatestCommonDivisorAbstract.java         | GreatestCommonDivisorAbstract.cs         |        |       |
| GreatestCommonDivisorModEval.java          | GreatestCommonDivisorModEval.cs          |        |       |
| GreatestCommonDivisorModular.java          | GreatestCommonDivisorModular.cs          |        |       |
| GreatestCommonDivisorPrimitive.java        | GreatestCommonDivisorPrimitive.cs        |        |       |
| GreatestCommonDivisorSimple.java           | GreatestCommonDivisorSimple.cs           |        |       |
| GreatestCommonDivisorSubres.java           | GreatestCommonDivisorSubres.cs           |        |       |
| HenselApprox.java                          | HenselApprox.cs                          |        |       |
| HenselMultUtil.java                        | HenselMultUtil.cs                        |        |       |
| HenselUtil.java                            | HenselUtil.cs                            |        |       |
| NoLiftingException.java                    | NoLiftingException.cs                    |        |       |
| PolyUfdUtil.java                           | PolyUfdUtil.cs                           |        |       |
| Quotient.java                              | Quotient.cs                              |        |       |
| QuotientRing.java                          | QuotientRing.cs                          |        |       |
| Squarefree.java                            | Squarefree.cs                            |        |       |
| SquarefreeAbstract.java                    | SquarefreeAbstract.cs                    |        |       |
| SquarefreeFactory.java                     | SquarefreeFactory.cs                     |        |       |
| SquarefreeFieldChar0.java                  | SquarefreeFieldChar0.cs                  |        |       |
| SquarefreeFieldCharP.java                  | SquarefreeFieldCharP.cs                  |        |       |
| SquarefreeFiniteFieldCharP.java            | SquarefreeFiniteFieldCharP.cs            |        |       |
| SquarefreeInfiniteAlgebraicFieldCharP.java | SquarefreeInfiniteAlgebraicFieldCharP.cs |        |       |
| SquarefreeInfiniteFieldCharP.java          | SquarefreeInfiniteFieldCharP.cs          |        |       |
| SquarefreeRingChar0.java                   | SquarefreeRingChar0.cs                   |        |       |

### Util (5 files)

| Java File                     | C# File                     | Status | Notes |
|-------------------------------|-----------------------------|--------|-------|
| CartesianProduct.java         | CartesianProduct.cs         |        |       |
| CartesianProductInfinite.java | CartesianProductInfinite.cs |        |       |
| KsubSet.java                  | KsubSet.cs                  |        |       |
| ListUtil.java                 | ListUtil.cs                 |        |       |
| LongIterable.java             | LongIterable.cs             |        |       |

### Vector (1 file)

| Java File        | C# File        | Status | Notes |
|------------------|----------------|--------|-------|
| BasicLinAlg.java | BasicLinAlg.cs |        |       |
