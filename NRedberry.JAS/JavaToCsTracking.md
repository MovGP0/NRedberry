﻿# Java to C# Scaffolding Tracking

This document tracks the scaffolding status of all Java files in the NRedberry.JAS project.

## Status Legend
- ✅ **Scaffolded** - Class skeleton with public methods/properties created, throwing NotImplementedException
- ✔️ **Complete** - Fully implemented

## Files

### Arith (12 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| BigComplex.java | BigComplex.cs | ✔️ Complete | |
| BigInteger.java | BigInteger.cs | ✔️ Complete | |
| BigRational.java | BigRational.cs | ✔️ Complete | |
| Combinatoric.java | Combinatoric.cs | ✔️ Complete | |
| ModInteger.java | ModInteger.cs | ✔️ Complete | |
| ModIntegerRing.java | ModIntegerRing.cs | ✔️ Complete | |
| ModLong.java | ModLong.cs | ✔️ Complete | |
| ModLongRing.java | ModLongRing.cs | ✔️ Complete | |
| Modular.java | Modular.cs | ✔️ Complete | |
| ModularNotInvertibleException.java | ModularNotInvertibleException.cs | ✔️ Complete | |
| ModularRingFactory.java | ModularRingFactory.cs | ✔️ Complete | |
| PrimeList.java | PrimeList.cs | ✔️ Complete | |

### Gb (4 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| GroebnerBase.java | GroebnerBase.cs | ✔️ Complete | |
| Reduction.java | Reduction.cs | ✔️ Complete | |
| ReductionAbstract.java | ReductionAbstract.cs | ✔️ Complete | |
| ReductionSeq.java | ReductionSeq.cs | ✔️ Complete | |

### Poly (24 files)
| Java File | C# File | Status    | Notes |
|-----------|---------|-----------|-------|
| AlgebraicNotInvertibleException.java | AlgebraicNotInvertibleException.cs | ✅ Scaffolded | |
| AlgebraicNumber.java | AlgebraicNumber.cs | ✅ Scaffolded | Algebraic number with GenPolynomial value |
| AlgebraicNumberRing.java | AlgebraicNumberRing.cs | ✅ Scaffolded | Factory for AlgebraicNumber |
| Complex.java | Complex.cs | ✅ Scaffolded | Complex numbers with real/imaginary parts |
| ComplexRing.java | ComplexRing.cs | ✅ Scaffolded | Factory for Complex numbers |
| ExpVector.java | ExpVector.cs | ✅ Scaffolded | Abstract class with key methods |
| ExpVectorByte.java | ExpVectorByte.cs | ✅ Scaffolded | Implements ExpVector with sbyte[] storage |
| ExpVectorInteger.java | ExpVectorInteger.cs | ✅ Scaffolded | Implements ExpVector with int[] storage |
| ExpVectorLong.java | ExpVectorLong.cs | ✅ Scaffolded | Implements ExpVector with long[] storage |
| ExpVectorPair.java | ExpVectorPair.cs | ✅ Scaffolded | Pair of ExpVectors for S-polynomials |
| ExpVectorShort.java | ExpVectorShort.cs | ✅ Scaffolded | Implements ExpVector with short[] storage |
| GenPolynomial.java | GenPolynomial.cs | ✅ Scaffolded | Generic with RingElem methods |
| GenPolynomialRing.java | GenPolynomialRing.cs | ✅ Scaffolded | Factory for GenPolynomial, n-variate |
| GenSolvablePolynomial.java | GenSolvablePolynomial.cs | ✅ Scaffolded | Solvable polynomial extending GenPolynomial |
| GenSolvablePolynomialRing.java | GenSolvablePolynomialRing.cs | ✅ Scaffolded | Factory for GenSolvablePolynomial |
| Monomial.java | Monomial.cs | ✅ Scaffolded | |
| OptimizedPolynomialList.java | OptimizedPolynomialList.cs | ✅ Scaffolded | Extends PolynomialList with optimized order |
| OrderedPolynomialList.java | OrderedPolynomialList.cs | ✅ Scaffolded | Sorted polynomial list with comparator |
| PolyIterator.java | PolyIterator.cs | ✅ Scaffolded | |
| PolynomialComparator.java | PolynomialComparator.cs | ✅ Scaffolded | IComparer for GenPolynomial |
| PolynomialList.java | PolynomialList.cs | ✅ Scaffolded | List with ring factory |
| PolyUtil.java | PolyUtil.cs | ✅ Scaffolded | Polynomial utilities and conversions |
| RelationTable.java | RelationTable.cs | ✅ Scaffolded | Relation table for solvable polynomials |
| TermOrder.java | TermOrder.cs | ✅ Scaffolded | Term ordering (LEX, GRLEX, IGRLEX, etc.) |
| TermOrderOptimization.java | TermOrderOptimization.cs | ✅ Scaffolded | Variable permutation optimization |

### Ps (5 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| Coefficients.java | Coefficients.cs | ✔️ Complete | |
| PolynomialTaylorFunction.java | PolynomialTaylorFunction.cs | ✔️ Complete | |
| TaylorFunction.java | TaylorFunction.cs | ✔️ Complete | |
| UnivPowerSeries.java | UnivPowerSeries.cs | ✔️ Complete | |
| UnivPowerSeriesRing.java | UnivPowerSeriesRing.cs | ✔️ Complete | |

### Structure (13 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| AbelianGroupElem.java | AbelianGroupElem.cs | ✔️ Complete | |
| AbelianGroupFactory.java | AbelianGroupFactory.cs | ✔️ Complete | |
| BinaryFunctor.java | BinaryFunctor.cs | ✔️ Complete | |
| Element.java | Element.cs | ✔️ Complete | |
| ElemFactory.java | ElemFactory.cs | ✔️ Complete | |
| GcdRingElem.java | GcdRingElem.cs | ✔️ Complete | |
| MonoidElem.java | MonoidElem.cs | ✔️ Complete | |
| MonoidFactory.java | MonoidFactory.cs | ✔️ Complete | |
| NotInvertibleException.java | NotInvertibleException.cs | ✔️ Complete | |
| Power.java | Power.cs | ✔️ Complete | |
| RingElem.java | RingElem.cs | ✔️ Complete | |
| RingFactory.java | RingFactory.cs | ✔️ Complete | |
| UnaryFunctor.java | UnaryFunctor.cs | ✔️ Complete | |

### Ufd (37 files)
| Java File | C# File | Status       | Notes |
|-----------|---------|--------------|-------|
| FactorAbsolute.java | FactorAbsolute.cs | ✅ Scaffolded | Abstract class for absolute factorization |
| FactorAbstract.java | FactorAbstract.cs | ✅ Scaffolded | Base abstract factorization class |
| FactorAlgebraic.java | FactorAlgebraic.cs | ✅ Scaffolded | Algebraic number coefficient factorization |
| FactorComplex.java | FactorComplex.cs | ✅ Scaffolded | Complex coefficient factorization |
| FactorFactory.java | FactorFactory.cs | ✅ Scaffolded | Factory for selecting factorization algorithms |
| FactorInteger.java | FactorInteger.cs | ✅ Scaffolded | Integer coefficient factorization (Hensel) |
| Factorization.java | Factorization.cs | ✅ Scaffolded | Factorization interface |
| FactorModular.java | FactorModular.cs | ✅ Scaffolded | Modular coefficient factorization |
| FactorQuotient.java | FactorQuotient.cs | ✅ Scaffolded | Quotient coefficient factorization |
| FactorRational.java | FactorRational.cs | ✅ Scaffolded | Rational coefficient factorization |
| Factors.java | Factors.cs | ✅ Scaffolded | Container for absolute factorization results |
| GCDFactory.java | GCDFactory.cs | ✅ Scaffolded | Factory for GCD algorithm selection |
| GreatestCommonDivisor.java | GreatestCommonDivisor.cs | ✅ Scaffolded | GCD interface with generic type parameter |
| GreatestCommonDivisorAbstract.java | GreatestCommonDivisorAbstract.cs | ✅ Scaffolded | Abstract GCD base with interface methods |
| GreatestCommonDivisorModEval.java | GreatestCommonDivisorModEval.cs | ✅ Scaffolded | GCD with modular evaluation algorithm |
| GreatestCommonDivisorModular.java | GreatestCommonDivisorModular.cs | ✅ Scaffolded | GCD with Chinese remainder algorithm |
| GreatestCommonDivisorPrimitive.java | GreatestCommonDivisorPrimitive.cs | ✅ Scaffolded | GCD with primitive polynomial remainder |
| GreatestCommonDivisorSimple.java | GreatestCommonDivisorSimple.cs | ✅ Scaffolded | GCD with monic polynomial remainder |
| GreatestCommonDivisorSubres.java | GreatestCommonDivisorSubres.cs | ✅ Scaffolded | GCD with subresultant remainder |
| HenselApprox.java | HenselApprox.cs | ✅ Scaffolded | Container for Hensel algorithm results |
| HenselMultUtil.java | HenselMultUtil.cs | ✅ Scaffolded | Hensel multivariate lifting utilities |
| HenselUtil.java | HenselUtil.cs | ✅ Scaffolded | Hensel univariate lifting utilities |
| NoLiftingException.java | NoLiftingException.cs | ✅ Scaffolded | |
| PolyUfdUtil.java | PolyUfdUtil.cs | ✅ Scaffolded | Polynomial UFD utilities and conversions |
| Quotient.java | Quotient.cs | ✅ Scaffolded | Quotient ring element (rational function) |
| QuotientRing.java | QuotientRing.cs | ✅ Scaffolded | Quotient ring factory with generic type |
| Squarefree.java | Squarefree.cs | ✅ Scaffolded | Squarefree decomposition interface |
| SquarefreeAbstract.java | SquarefreeAbstract.cs | ✅ Scaffolded | Abstract squarefree base with methods |
| SquarefreeFactory.java | SquarefreeFactory.cs | ✅ Scaffolded | Factory for squarefree decomposition |
| SquarefreeFieldChar0.java | SquarefreeFieldChar0.cs | ✅ Scaffolded | Squarefree for fields char 0 |
| SquarefreeFieldCharP.java | SquarefreeFieldCharP.cs | ✅ Scaffolded | Abstract squarefree for fields char p |
| SquarefreeFiniteFieldCharP.java | SquarefreeFiniteFieldCharP.cs | ✅ Scaffolded | Squarefree for finite fields char p |
| SquarefreeInfiniteAlgebraicFieldCharP.java | SquarefreeInfiniteAlgebraicFieldCharP.cs | ✅ Scaffolded | Squarefree for infinite algebraic fields |
| SquarefreeInfiniteFieldCharP.java | SquarefreeInfiniteFieldCharP.cs | ✅ Scaffolded | Squarefree for infinite fields char p |
| SquarefreeRingChar0.java | SquarefreeRingChar0.cs | ✅ Scaffolded | Squarefree for rings char 0 |

### Util (5 files)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| CartesianProduct.java | CartesianProduct.cs | ✔️ Complete | |
| CartesianProductInfinite.java | CartesianProductInfinite.cs | ✔️ Complete | |
| KsubSet.java | KsubSet.cs | ✔️ Complete | |
| ListUtil.java | ListUtil.cs | ✔️ Complete | |
| LongIterable.java | LongIterable.cs | ✔️ Complete | |

### Vector (1 file)
| Java File | C# File | Status | Notes |
|-----------|---------|--------|-------|
| BasicLinAlg.java | BasicLinAlg.cs | ✔️ Complete | |
